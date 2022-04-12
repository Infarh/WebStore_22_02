using Microsoft.AspNetCore.Mvc;

using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers
{
    //[Route("api/[controller]")] // http://localhost:5001/api/values
    [ApiController]
    [Route(WebAPIAddresses.V1.Values)] // http://localhost:5001/api/values
    public class ValuesController : ControllerBase
    {
        private static readonly Dictionary<int, string> _Values = Enumerable.Range(1, 10)
           .Select(i => (Id: i, Value: $"Value-{i}"))
           .ToDictionary(v => v.Id, v => v.Value);

        private readonly ILogger<ValuesController> _Logger;

        public ValuesController(ILogger<ValuesController> Logger) => _Logger = Logger;

        [HttpGet]
        public IActionResult GetAll()
        {
            var values = _Values.Values;
            return Ok(values);
        }

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

            _Logger.LogInformation("Добавлено значение {0} с Id:{1}", Value, id);

            return CreatedAtAction(nameof(GetById), new { Id = id }, Value);
        }

        [HttpPut("{Id}")] // PUT -> http://localhost:5001/api/values
        public IActionResult Edit(int Id, [FromBody] string Value)
        {
            if (!_Values.ContainsKey(Id))
            {
                //_Logger.LogWarning($"Попытка редактирования отсутствующего значения с id:{Id}");
                _Logger.LogWarning("Попытка редактирования отсутствующего значения с id:{0}", Id);
                return NotFound(new { Id });
            }

            var old_value = _Values[Id];
            _Values[Id] = Value;

            _Logger.LogInformation("Выполнено изменения значения с id:{0} с {1} на {2}",
                Id, old_value, Value);

            return Ok(new { Value, OldValue = old_value });
        }

        [HttpDelete("{Id}")] // DELETE -> http://localhost:5001/api/values/42
        public IActionResult Delete(int Id)
        {
            if (!_Values.ContainsKey(Id))
            {
                _Logger.LogWarning("Попытка удаления отсутствующего значения с id:{0}", Id);
                return NotFound(new { Id });
            }

            var value = _Values[Id];
            _Values.Remove(Id);

            _Logger.LogInformation("Значение {0} с id:{1} удалено", value, Id);

            return Ok(new { Value = value });
        }
    }
}
