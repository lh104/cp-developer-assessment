﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TodoList.Data.Entities;

namespace TodoList.Data.Context
{
    public class TodoContext : DbContext
    {
        public TodoContext( DbContextOptions<TodoContext> options )
            : base( options ) {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

    }
}