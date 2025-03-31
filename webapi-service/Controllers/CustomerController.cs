using AutoMapper;
using webapi.ActionFilters;
using webapi.Dtos;
using webapi.Exceptions;
using webapi.Models;
using webapi.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;


namespace webapi.Controllers
{
    [Route("api/v1/Customers/")]
    [ApiController]
    public class CustomerController : WebAppControllerBase<CustomerController>
    {
        private readonly ICustomerRepository _repository;
        //private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository repository, IMapper mapper, ILogger<CustomerController> logger) : 
            base(logger, mapper)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("/$/GetAll")]
        public async Task<ObjectResult> GetAllCustomers()
        {
            logger.LogInformation("--> Getting all customers.");


            var result = await _repository.GetAllAsync();


            var data = mapper.Map<IList<CustomerReadDto>>(result);

            var response = new ResponseDto<IList<CustomerReadDto>>()
            {
                Data = data,
                Title = CONST_TITLE_SUCCESS_RESPONSE
            };

            return Ok(response);


         }

        [HttpPut()]
        [Route("{customerId}/$/Update")]
        public async Task<ObjectResult> UpdateTake(int customerId, [FromBody] CustomerUpdateDto dtoRequest)
        {

            logger.LogInformation("Modify Customer.");

            var existingEntity = await _repository.GetByIdAsync(customerId);

            var entity = mapper.Map<Customer>(dtoRequest);
            entity.Id = existingEntity.Id;  

            //save and persists to database
            await _repository.UpdateAsync(entity);


            //build the response
            var response = new ResponseDto<bool>()
            {

                Data = true,
                Title = CONST_TITLE_SUCCESS_RESPONSE
            };


            return Ok(response);

        }

        



        [HttpPost()]
        [Route("$/Add")]
        public async Task<ObjectResult> AddCustomer([FromBody] CustomerCreateDto dtoRequest)
        {

            logger.LogInformation("Add Customer.");


            //use AutoMapper to map the dto to persistent entity
            var entity = mapper.Map<Customer>(dtoRequest);

            //save to database

            await _repository.CreateAsync(entity);


            //build the response
            var response = new ResponseDto<bool>()
            {

                Data = true,
                Title = CONST_TITLE_SUCCESS_RESPONSE
            };


            return Ok(response);

        }

        [HttpDelete()]
        [Route("{customerId}/$/Delete")]
        public async Task<ObjectResult> DeleteCustomer(int customerId)
        {

            logger.LogInformation("Delete a Customer ");

            var customer = _repository.GetById(customerId);

            if (customer == null)
            {
                // If customer doesn't exist, return a Not Found (404) status with an error message
                throw new WebApiServiceException("This customer can't be found");
            }


            //delete from database
            await _repository.DeleteByIdAsync(customerId);


            //build the response
            var response = new ResponseDto<bool>()
            {

                Data = true,
                Title = CONST_TITLE_SUCCESS_RESPONSE
            };


            return Ok(response);

        }


    }
}

