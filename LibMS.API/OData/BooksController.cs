using System;
using LibMS.Data.Entities;
using LibMS.Data.Repositories;
using LibMS.Persistance.EFConcreteRepositories;
using LibMS.Business.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace LibMS.API.OData
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

