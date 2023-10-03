using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TodoList.Common.Extensions;
using TodoList.Service.Interfaces;
using TodoList.Api.Mapping;
using TodoList.Api.Model;

namespace TodoList.Api.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _service;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController( ITodoItemService service, ILogger<TodoItemsController> logger ) {
            _service = service;
            _logger = logger;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems() {
            this._logger.LogDebug( "GetListAsync() called" );
            try {
                var results = await _service.GetListAsync( x => !x.IsCompleted );
                return Ok( results.ToDC() );
            } catch ( Exception ex ) {
                var message = $"Failed to get TodoItem Error: {ex.GetErrorMessage()}";
                this._logger.LogError( message, ex );
                return StatusCode( StatusCodes.Status500InternalServerError, message );
            }
        }

        // GET: api/TodoItems/...
        [HttpGet( "{id}" )]
        [ProducesResponseType( StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        public async Task<IActionResult> GetTodoItem( Guid id ) {
            this._logger.LogDebug( $"GetOneAsync() called with id: {id}" );
            try {
                var result = await _service.GetOneAsync( id );

                if ( result == null ) {
                    var message = $"TodoItem with id: '{id}' does not exist!";
                    this._logger.LogError( message );
                    return StatusCode( StatusCodes.Status404NotFound, new ApiResponse { Id = id.ToString(), Message = message, Result = false } );
                }

                return Ok( result.ToDC() );
            } catch ( Exception ex ) {
                var message = $"Failed to get the TodoItem with id: {id}";
                this._logger.LogError( $"{message}.  Error: {ex.GetErrorMessage()}" );
                return StatusCode( StatusCodes.Status500InternalServerError, new ApiResponse { Id = id.ToString(), Message = message, Result = false } );
            }
        }

        // PUT: api/TodoItems/... 
        [HttpPut( "{id}" )]
        public async Task<IActionResult> PutTodoItem( Guid id, TodoItemDto todoItem ) {
            try {

                if ( id != todoItem.Id ) {
                    return BadRequest();
                }

                await _service.UpdateAsync( todoItem.ToData() );

            } catch ( DbUpdateConcurrencyException ex) {
                //return NotFound();
                return StatusCode( StatusCodes.Status404NotFound, new ApiResponse { Message = ex.GetErrorMessage() } );
                throw;
            } catch ( InvalidOperationException ex ) {
                return StatusCode( StatusCodes.Status400BadRequest, new ApiResponse { Message = ex.GetErrorMessage() } );
                throw;
            }

            return NoContent();
        }

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem( TodoItemDto todoItem ) {

            try {
                if ( string.IsNullOrEmpty( todoItem?.Description ) ) {
                    return BadRequest( "Description is required" );
                }

                var createdEntity = await _service.CreateAsync( todoItem.ToData() );
                return CreatedAtAction( nameof( GetTodoItem ), new { id = createdEntity.Id }, createdEntity.ToDC() );

            } catch ( InvalidOperationException ex) {
                return StatusCode( StatusCodes.Status400BadRequest, new ApiResponse { Message = ex.GetErrorMessage() } );
                throw;
            }
        }

    }
}
