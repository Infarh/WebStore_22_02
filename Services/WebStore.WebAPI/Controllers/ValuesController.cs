﻿using Microsoft.AspNetCore.Mvc;

namespace WebStore.WebAPI.Controllers
{
    //[Route("api/[controller]")] // http://localhost:5001/api/values
    [Route("api/values")] // http://localhost:5001/api/values
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly Dictionary<int, string> _Values = Enumerable.Range(1, 10)
           .Select(i => (Id: i, Value: $"Value-{i}"))
           .ToDictionary(v => v.Id, v => v.Value);

        private readonly ILogger<ValuesController> _Logger;

        public ValuesController(ILogger<ValuesController> Logger) => _Logger = Logger;

        [HttpGet]
        public IActionResult GetAll() => Ok(_Values.Select(v => new { v.Key, v.Value }));

        [HttpGet("{Id}")]
        public IActionResult GetById(int Id)
        {
            //if (!_Values.ContainsKey(Id))
            //    return NotFound();
            //return Ok(_Values[Id]);

            if (_Values.TryGetValue(Id, out var value))
                return Ok(value);
            return NotFound(new { Id });
        }

        [HttpGet("count")]
        public int Count() => _Values.Count;

        [HttpPost]          // POST -> http://localhost:5001/api/values
        [HttpPost("add")]   // POST -> http://localhost:5001/api/values/add
        public IActionResult Add([FromBody] string Value)
        {
            var id = _Values.Count == 0 ? 1 : _Values.Keys.Max() + 1;
            _Values[id] = Value;

            return CreatedAtAction(nameof(GetById), new { id }, Value);
        }

        [HttpPut("{Id}")] // PUT -> http://localhost:5001/api/values
        public IActionResult Edit(int Id, [FromBody] string Value)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            _Values[Id] = Value;

            return Ok(new { Id, Value });
        }

        [HttpDelete("{Id}")] // DELETE -> http://localhost:5001/api/values/42
        public IActionResult Delete(int Id)
        {
            if (!_Values.ContainsKey(Id))
                return NotFound();

            var value = _Values[Id];
            _Values.Remove(Id);

            return Ok(value);
        }
    }
}
