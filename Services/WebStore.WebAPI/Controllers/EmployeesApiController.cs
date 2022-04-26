﻿using Microsoft.AspNetCore.Mvc;

using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

/// <summary>Сотрудники</summary>
[ApiController]
[Route(WebAPIAddresses.V1.Employees)]
//[Produces("application/json")]
//[Produces("application/xml")]
public class EmployeesApiController : ControllerBase
{
    private readonly IEmployeesData _EmployeesData;
    private readonly ILogger<EmployeesApiController> _Logger;

    public EmployeesApiController(IEmployeesData EmployeesData, ILogger<EmployeesApiController> Logger)
    {
        _EmployeesData = EmployeesData;
        _Logger = Logger;
    }

    /// <summary>Все сотрудники</summary>
    /// <returns>Возвращает список всех сотрудников</returns>
    /// <response code="200">Ok</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _EmployeesData.GetAllAsync();
        if (employees.Any())
            return Ok(employees);
        return NoContent();
    }

    /// <summary>Сотрудник с заданным идентификатором</summary>
    /// <param name="Id">Искомый идентификатор сотрудника</param>
    /// <returns>Сотрудник с заданным идентификатором, либо <c>null</c> в случае его необнаружения</returns>
    [HttpGet("{Id:int}")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int Id)
    {
        var employee = await _EmployeesData.GetByIdAsync(Id);
        if (employee is null)
            //return NoContent();
            return NotFound();
        return Ok(employee);
    }

    /// <summary>Добавление сотрудника</summary>
    /// <param name="employee">Новый сотрудник</param>
    /// <returns>Созданный сотрудник</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
    public async Task<IActionResult>Add(Employee employee)
    {
        var id = await _EmployeesData.AddAsync(employee);
        _Logger.LogInformation("Сотрудник {0} добавлен с идентификатором {1}", employee, id);
        return CreatedAtAction(nameof(GetById), new { Id = id }, employee);
    }

    /// <summary>Редактирование сотрудника</summary>
    /// <param name="employee">Информация для редактирования сотрудника</param>
    /// <returns>true если редактирование выполнено успешно</returns>
    [HttpPut]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult>Edit(Employee employee)
    {
        var success = await _EmployeesData.EditAsync(employee);
        if (success)
            _Logger.LogInformation("Сотрудник {0} отредактирован", employee);
        else
            _Logger.LogWarning("Проблема при редактировании сотрудника {0}", employee);

        return Ok(success);
    }

    /// <summary>Удаление сотрудника</summary>
    /// <param name="Id">Идентификатор удаляемого сотрудника</param>
    /// <returns><c>true</c> если сотрудник был удалён</returns>
    [HttpDelete("{Id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status404NotFound)]
    public async Task<IActionResult>Delete(int Id)
    {
        var result = await _EmployeesData.DeleteAsync(Id);
        if (result)
        {
            _Logger.LogInformation("Сотрудник с id:{0} удалён", Id);
            return Ok(true);
        }

        _Logger.LogWarning("Сотрудник с id:{0} при удалении не найден", Id);
        return NotFound(false);
    }
}
