using System;
using LibMs.Data.Entities;
using LibMs.Data.Repositories;
using LibMs.Persistance.EFConcreteRepositories;
using LibMS.Business.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace LibMs.API.OData
{
    [ApiController]
    [Route("odata/[controller]")]
    public class BooksController : ODataController
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var books = _bookService.GetBooks();
            return Ok(books);
        }
    }
}

