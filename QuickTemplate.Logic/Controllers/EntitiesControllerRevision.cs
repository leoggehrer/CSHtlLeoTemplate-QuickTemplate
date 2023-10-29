//@BaseCode
//MdStart
#if ACCOUNT_ON && REVISION_ON
namespace QuickTemplate.Logic.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a class that handles the history of entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TOutModel">The type of output model.</typeparam>
    partial class EntitiesController<TEntity, TOutModel>
    {
        private enum RevisionType
        {
            Insert,
            Update,
            Delete,
        }
        private record HistoryItem(IdType IdentityId, RevisionType RevisionType, DateTime ActionTime, Entities.EntityObject Entity, string? JsonData);
        private readonly List<HistoryItem> historyItems = new();
        
        /// <summary>
        /// Executed after an insert operation.
        /// </summary>
        /// <param name="entity">The entity that was inserted.</param>
        partial void AfterExecuteInsert(TEntity entity)
        {
            if (entity.GetType() != typeof(Entities.Revision.History))
            {
                var loginSession = Modules.Account.AccountManager.QueryLoginSession(SessionToken);
                
                if (loginSession != null)
                {
                    historyItems.Add(new HistoryItem(loginSession.IdentityId, RevisionType.Insert, DateTime.Now, entity, null));
                }
            }
        }
        /// <summary>
        /// This method is called before executing an update operation on the TEntity entity.
        /// </summary>
        /// <param name="entity">
        /// The entity object to be updated.
        /// </param>
        partial void BeforeExecuteUpdate(TEntity entity)
        {
            var historyItem = historyItems.FirstOrDefault(e => e.Entity.Id == entity.Id);
            
            if (historyItem == null)
            {
                var oriEntity = EntitySet.Where(e => e.Id == entity.Id).AsNoTracking().FirstOrDefault();
                
                if (oriEntity != null)
                {
                    var loginSession = Modules.Account.AccountManager.QueryLoginSession(SessionToken);
                    
                    if (loginSession != null)
                    {
                        historyItems.Add(new HistoryItem(loginSession.IdentityId, RevisionType.Update, DateTime.Now, entity, JsonSerializer.Serialize(oriEntity, new JsonSerializerOptions() { MaxDepth = 0, })));
                    }
                }
            }
        }
        /// <summary>
        /// Performs pre-delete operations on the specified entity.
        /// </summary>
        /// <param name="entity">The entity to be deleted</param>
        // code implementation
        partial void BeforeExecuteDelete(TEntity entity)
        {
            var loginSession = Modules.Account.AccountManager.QueryLoginSession(SessionToken);
            
            if (loginSession != null)
            {
                historyItems.Add(new HistoryItem(loginSession.IdentityId, RevisionType.Delete, DateTime.Now, entity, JsonSerializer.Serialize(entity, new JsonSerializerOptions() { MaxDepth = 0 })));
            }
        }
        /// <summary>
        /// This method is called after the execution of SaveChanges.
        /// It processes the history items and inserts them into the revision histories table.
        /// </summary>
        /// <remarks>
        /// The method checks if there are any history items. If there are, it starts a new Task
        /// to process the items and insert them into the database using the Revision.HistoriesController class.
        /// The Task runs asynchronously to avoid blocking the caller.
        /// </remarks>
        /// <exception cref="System.Exception">
        /// Throws an exception if an error occurs while inserting the history items.
        /// </exception>
        /// <seealso cref="AfterExecuteSaveChangesAsync"/>
        partial void AfterExecuteSaveChanges()
        {
            if (historyItems.Any())
            {
                Task.Run(async () =>
                {
                    using var ctrl = new Revision.HistoriesController();
                    
                    ctrl.SessionToken = Modules.Security.Authorization.SystemAuthorizationToken;
                    foreach (var item in historyItems)
                    {
                        var history = new Entities.Revision.History()
                        {
                            IdentityId = item.IdentityId,
                            ActionType = item.RevisionType.ToString(),
                            ActionTime = item.ActionTime,
                            SubjectName = item.Entity.GetType().Name,
                            SubjectId = item.Entity.Id,
                            JsonData = item.JsonData ?? string.Empty,
                        };
                        _ = await ctrl.ExecuteInsertAsync(history).ConfigureAwait(false);
                    }
                    _ = await ctrl.SaveChangesAsync().ConfigureAwait(false);
                }).Wait();
                historyItems.Clear();
            }
        }
    }
}
#endif
//MdEnd
