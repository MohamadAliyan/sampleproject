using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sample.Data.Entities;
using Sample.Service.Abstract;
using Sample.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Util;

namespace Sample.Api.Controllers
{
    
    public class BaseApiController<TInput, TResult, TChild> : Controller
        where TInput : BaseEntity
        where TResult : BaseServiceModel
    {
        private readonly IService<TInput, TResult> _Service;
        public  ILogger<TChild> _logger;

        public int CurrentUserId
        {
            get
            {
                try
                {

                    return 1;
                }
                catch (Exception ex)
                {
                }
                return 0;
            }
        }

    


        public BaseApiController(IService<TInput, TResult> service, ILogger<TChild> logger = null)
        {
            _Service = service;
            _logger = logger;
        }

        #region Crud...
        [HttpGet]
        public  virtual IActionResult GetAllList()
        {
            var list = _Service.GetAll();

            return Ok(list);
        }

        [HttpPost]
        public virtual IActionResult GetAllBySearch(int pageNumber = 1, int pageSize = 10,[FromBody] Dictionary<string, dynamic> filterParams = null)
        {
            var list = _Service.GetAllbySearch(pageNumber, allIncluded: false,filterParams:filterParams);

            return Ok(list);
        }

        [HttpGet]
        public virtual IActionResult GetAll(int pageNumber = 1)
        {
            var list = _Service.GetAllbySearch(pageNumber, allIncluded: true);

            return Ok(list);
        }

        [HttpGet]
        public virtual IActionResult Get(long id)
        {
            var model = _Service.Get(id);
            return Ok(model);
        }

        [HttpPost]
        public virtual IActionResult Create([FromBody] TResult model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _Service.Insert(model, CurrentUserId);
                    return Ok();
                }
                else
                {
                    _logger?.LogError(LoggingEvents.InsertItem, "Create Error", ModelState);
                    _logger?.LogError(LoggingEvents.InsertItem, JsonConvert.SerializeObject(ModelState), null);

                    return StatusCode(406, ModelState.Keys.ToString());//Not Acceptable
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(LoggingEvents.InsertItem, ex, null);
                return BadRequest();
            }
            return Ok();

        }

        [HttpPost]
        public virtual IActionResult Edit([FromBody]TResult model)
        {
            try
            {
                _Service.Update(model, CurrentUserId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.UpdateItem, ex, null);
                return BadRequest(); 
            }
        }

        [HttpGet]
        public virtual IActionResult Delete(long id)
        {
            try
            {
                _Service.Delete(id, CurrentUserId);
                return Ok();
            }
            catch (Exception ex)
            {
                // _logger.LogError(LoggingEvents.DeleteItem, ex, null);
                return BadRequest();
            }
        }

        #endregion

    }
}