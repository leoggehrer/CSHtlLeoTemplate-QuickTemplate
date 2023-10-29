//@BaseCode
//MdStart
#if IDINT_ON
    global using IdType = System.Int32;
#elif IDLONG_ON
    global using IdType = System.Int64;
#elif IDGUID_ON
    global using IdType = System.Guid;
#else
    global using IdType = System.Int32;
#endif
global using CommonBase;
global using CommonBase.Extensions;
global using BaseContracts = CommonBase.Contracts;
global using BaseModels = CommonBase.Models;
global using BaseServices = CommonBase.Services;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using Microsoft.EntityFrameworkCore;
//MdEnd
