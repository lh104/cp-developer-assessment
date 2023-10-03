using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TodoList.Data.Entities;
using TodoList.Data.Interfaces;
using TodoList.Service.Services;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class TodoItemServiceTests
    {
        private readonly Mock<ILogger<TodoItem>> todoItemLoggerMock;
        private readonly Mock<ITodoItemRepository> todoItemRepositoryMock;
        private readonly TodoItemService todoItemService;


        public TodoItemServiceTests() {

            todoItemLoggerMock = new Mock<ILogger<TodoItem>>();
            todoItemRepositoryMock = new Mock<ITodoItemRepository>();
            todoItemService = new TodoItemService( todoItemLoggerMock.Object,todoItemRepositoryMock.Object );
        }

        [Fact]
        public async void GetListAsync_ShouldReturnAListOfTodoItems_WhenTodoItemsExist() {
            var todoItems = CreateTodoItemList();

            todoItemRepositoryMock.Setup( c => c.GetListAsync(It.IsAny<Expression<Func<TodoItem, bool>>>())).ReturnsAsync( todoItems );

            var result = await todoItemService.GetListAsync();

            Assert.NotNull( result );
            Assert.IsType<List<TodoItem>>( result );
        }

        [Fact]
        public async void GetListAsync_ShouldReturnEmptyList_WhenNoTodoItemExists() {
            todoItemRepositoryMock.Setup( c => c.GetListAsync(It.IsAny<Expression<Func<TodoItem, bool>>>()) )
                .ReturnsAsync( (List<TodoItem>)null );

            var result = await todoItemService.GetListAsync();

            Assert.Null( result );
        }

        [Fact]
        public async void GetListAsync_ShouldCallGetListAsyncFromRepository_OnlyOnce() {
            todoItemRepositoryMock.Setup( c => c.GetListAsync(It.IsAny<Expression<Func<TodoItem, bool>>>()))
               .ReturnsAsync( new List<TodoItem>());

            await todoItemService.GetListAsync();

            todoItemRepositoryMock.Verify( mock => mock.GetListAsync(It.IsAny<Expression<Func<TodoItem, bool>>>()), Times.Once );
        }

        [Fact]
        public async void GetOneAsyncById_ShouldReturnTodoItem_WhenTodoItemExist() {
            var todoItem = CreateTodoItem();

            todoItemRepositoryMock.Setup( c => c.GetOneAsync(todoItem.Id) )
               .ReturnsAsync(todoItem);

            var result = await todoItemService.GetOneAsync( todoItem.Id );

            Assert.NotNull( result );
            Assert.IsType<TodoItem>( result );
        }

        [Fact]
        public async void GetOneAsyncById_ShouldReturnNull_WhenTodoItemNotExists() {
            var todoItem = CreateTodoItem();

            todoItemRepositoryMock.Setup( c => c.GetOneAsync(new Guid("cb53a06e-1dbf-4b47-a987-7b47e9f02eb9")))
              .ReturnsAsync( (TodoItem)null );

            var result = await todoItemService.GetOneAsync(new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9"));

            Assert.Null( result );
        }

        [Fact]
        public async void GetOneAsyncById_ShouldCallGetOneAsyncByIdFromRepository_OnlyOnce() {
            todoItemRepositoryMock.Setup( c => c.GetOneAsync( new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ) ) )
                .ReturnsAsync( new TodoItem() );

            await todoItemService.GetOneAsync( new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ) );

            todoItemRepositoryMock.Verify( mock => mock.GetOneAsync( new Guid( "cb53a06e-1dbf-4b47-a987-7b47e9f02eb9" ) ), Times.Once );
        }

        [Fact]
        public async void Create_ShouldCreateTodoItem_WhenTodoItemDescriptionDoesNotExist() {
            var todoItem = CreateTodoItem();

            todoItemRepositoryMock.Setup( c => c.CreateAsync( todoItem )).ReturnsAsync( todoItem );


            var result = await todoItemService.CreateAsync( todoItem );

            Assert.NotNull( result );
            Assert.IsType<TodoItem>( result );
        }

        [Fact]
        public async System.Threading.Tasks.Task Create_ShouldNotCreateTodoItem_WhenTodoItemDescriptionExist() {
            var todoItem = CreateTodoItem();

            todoItemRepositoryMock.Setup( c => c.GetExistsAsync( It.IsAny<Expression<Func<TodoItem, bool>>>() ) ).ReturnsAsync( true );

            await Assert.ThrowsAnyAsync<InvalidOperationException>( () => todoItemService.CreateAsync( todoItem ) );
        }

        [Fact]
        public async System.Threading.Tasks.Task Create_ShouldCallCreateAsyncFromRepository_OnlyOnce() {
            var todoItem = CreateTodoItem();

            todoItemRepositoryMock.Setup( c => c.CreateAsync( todoItem ) ).ReturnsAsync( todoItem );


            var result = await todoItemService.CreateAsync( todoItem );

            todoItemRepositoryMock.Verify( mock => mock.CreateAsync( todoItem ), Times.Once );
        }

        [Fact]
        public async void Update_ShouldUpdateTodoItem_WhenTodoItemExistAndNoDuplicateDescription() {
            var todoItem = CreateTodoItem();

            todoItemRepositoryMock.SetupSequence( c => c.GetExistsAsync( It.IsAny<Expression<Func<TodoItem, bool>>>() ) ).ReturnsAsync( true ).ReturnsAsync( false );
            todoItemRepositoryMock.Setup( c => c.UpdateAsync( todoItem ) ).ReturnsAsync( todoItem );


            var result = await todoItemService.UpdateAsync( todoItem );

            Assert.NotNull( result );
            Assert.IsType<TodoItem>( result );
        }

        [Fact]
        public async void Update_ShouldNotUpdateTodoItem_WhenTodoItemNotExists() {
            var todoItem = CreateTodoItem();

            todoItemRepositoryMock.SetupSequence( c => c.GetExistsAsync( It.IsAny<Expression<Func<TodoItem, bool>>>() ) ).ReturnsAsync( false );

            await Assert.ThrowsAnyAsync<DbUpdateConcurrencyException>( () => todoItemService.UpdateAsync( todoItem ) );
        }

        [Fact]
        public async void Update_ShouldNotUpdateTodoItem_WhenTodoItemExistsButDuplicateDescriptionExists() {
            var todoItem = CreateTodoItem();

            todoItemRepositoryMock.SetupSequence( c => c.GetExistsAsync( It.IsAny<Expression<Func<TodoItem, bool>>>() ) ).ReturnsAsync( true ).ReturnsAsync( true );

            await Assert.ThrowsAnyAsync<InvalidOperationException>( () => todoItemService.UpdateAsync( todoItem ) );
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
