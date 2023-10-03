using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TodoList.Api.Controllers;
using TodoList.Api.Mapping;
using TodoList.Api.Model;
using TodoList.Data.Entities;
using TodoList.Service.Interfaces;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class TodoItemsControllerTest
    {
        private readonly Mock<ILogger<TodoItemsController>> todoItemLoggerMock;
        private readonly Mock<ITodoItemService> todoItemServiceMock;
        private readonly TodoItemsController controller;

        public TodoItemsControllerTest() {
            Bootstrapper.Bootstrap();
            this.todoItemLoggerMock = new Mock<ILogger<TodoItemsController>>();
            this.todoItemServiceMock = new Mock< ITodoItemService >();
            this.controller = new TodoItemsController( todoItemServiceMock.Object, todoItemLoggerMock.Object );
        }

        [Fact]
        public async void GetTodoItems_ShouldReturnOk_WhenTodoItemsExist() {
            var todoItems = CreateTodoItemList();

            // Arrange
            todoItemServiceMock.Setup( c => c.GetListAsync( It.IsAny<Expression<Func<TodoItem, bool>>>() ) ).ReturnsAsync( todoItems );

            var result = await controller.GetTodoItems();

            // Assert
            Assert.IsType<OkObjectResult>( result );
        }

        [Fact]
        public async void GetTodoItem_ShouldReturnOk_WhenTodoItemsNotExist() {

            // Arrange
            todoItemServiceMock.Setup( c => c.GetListAsync(It.IsAny<Expression<Func<TodoItem, bool>>>() ) ).ReturnsAsync( new List<TodoItem>() );

            var result = await controller.GetTodoItems();

            // Assert
            Assert.IsType<OkObjectResult>( result );
        }

        [Fact]
        public async void GetTodoItems_ShouldCallGetListAsync_OnlyOnce() {
            var todoItems = CreateTodoItemList();

            // Arrange
            todoItemServiceMock.Setup( c => c.GetListAsync( It.IsAny<Expression<Func<TodoItem, bool>>>() ) ).ReturnsAsync( todoItems );

            var result = await controller.GetTodoItems();

            // Assert
            todoItemServiceMock.Verify( mock => mock.GetListAsync( It.IsAny<Expression<Func<TodoItem, bool>>>() ), Times.Once );
        }

        [Fact]
        public async void GetTodoItem_ShouldReturn_WhenTodoItemExist() {
            var todoItem = CreateTodoItem();

            // Arrange
            todoItemServiceMock.Setup( c => c.GetOneAsync( new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ) ) ).ReturnsAsync( todoItem );

            var result = await controller.GetTodoItem( new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ) );

            // Assert
            Assert.IsType<OkObjectResult>( result );
        }

        [Fact]
        public async void GetTodoItem_ShouldNotReturn_WhenTodoItemNotExist() {
            var todoItem = CreateTodoItem();

            // Arrange
            todoItemServiceMock.Setup( c => c.GetOneAsync( new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ) ) ).ReturnsAsync( (TodoItem)null );

            var result = await controller.GetTodoItem( new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ) ) as ObjectResult;

            // Assert
            Assert.Equivalent( 404, result.StatusCode);
        }

        [Fact]
        public async void GetTodoItem_ShouldCallGetOneAsync_OnlyOnce() {
            var todoItem = CreateTodoItem();

            // Arrange
            todoItemServiceMock.Setup( c => c.GetOneAsync( new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ) ) ).ReturnsAsync( todoItem );

            var result = await controller.GetTodoItem( new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ) );

            // Assert
            todoItemServiceMock.Verify( mock => mock.GetOneAsync(new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" )), Times.Once );
        }

        [Fact]
        public async void Create_ShouldReturnOk_WhenTodoItemCreated() {
            var todoItem = CreateTodoItem();
            var todoItemDto = new TodoItemDto() { Description = todoItem.Description };

            todoItemServiceMock.Setup( c => c.CreateAsync( It.IsAny<TodoItem>() ) ).ReturnsAsync( todoItem );

            var result = await controller.PostTodoItem( todoItemDto );

            Assert.IsType<CreatedAtActionResult>( result );
        }

        [Fact]
        public async void Create_ShouldReturnBadRequest_WhenTodoItemHasNoDescription() {
            var todoItemDto = new TodoItemDto();

            var result = await controller.PostTodoItem( todoItemDto );

            Assert.IsType<BadRequestObjectResult>( result );
        }

        [Fact]
        public async void Update_ShouldReturnBadRequest_WhenRouteIdAndEntityIdAreNotTheSame() {
            var todoItemDto = new TodoItemDto() { Id = new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ) };

            var result = await controller.PutTodoItem( new Guid( "c8764ab6-6245-11ee-8c99-0242ac120002" ), todoItemDto );

            Assert.IsType<BadRequestResult>( result );
        }

        [Fact]
        public async void Update_ShouldReturnNoContent_WhenTodoItemUpdated() {
            var todoItem = CreateTodoItem();
            var todoItemDto = new TodoItemDto() {
                Id = new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ),
                Description = "This is my first Todo Item",
                IsCompleted = false
            };
            todoItemServiceMock.Setup( c => c.UpdateAsync( It.IsAny<TodoItem>() ) ).ReturnsAsync( todoItem );
            var result = await controller.PutTodoItem( new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ), todoItemDto );

            Assert.IsType<NoContentResult>( result );
        }

        private List<TodoItem> CreateTodoItemList() {
            return new List<TodoItem> {
                new TodoItem { Id = new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9"), Description = "This is my first Todo Item", IsCompleted = false },
                new TodoItem {Id = new Guid( "db5437bb-6d7c-4b9f-81cc-129b9da2c725"), Description = "This is my second Todo Item", IsCompleted = false }
            };
        }

        private TodoItem CreateTodoItem() {
            return new TodoItem() {
                Id = new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ),
                Description = "This is my first Todo Item",
                IsCompleted = false
            };
        }
    }
}
