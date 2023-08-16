using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using Ticketinho.Common.DTOs.Demand;
using Ticketinho.Common.DTOs.Tickets;
using Ticketinho.Service.Demands;
using Ticketinho.Validation.Demand;
using Ticketinho.Validation.Tickets;

namespace Ticketinho.Functions
{
    public class DemandFunctions
    {
        private readonly IDemandService _demandService;

        public DemandFunctions(IDemandService demandService)
        {
            _demandService = demandService;
        }

        [Function("Add demand")]
        public async Task<HttpResponseData> Add([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "demands")] HttpRequestData req)
        {
            var createRequest = await req.GetJsonBody<CreateDemandRequestDto, CreateDemandRequestValidation>();
            if (!createRequest.IsValid)
            {
                return await createRequest.ToBadRequest(req);
            }

            var demandId = await _demandService.AddAsync(createRequest.Value.UserId,
                                                         createRequest.Value.Zone,
                                                         createRequest.Value.Type,
                                                         createRequest.Value.Price);

            return await req.CreateResponseWithContentAsync(demandId);
        }

        [Function("Delete demand")]
        public async Task<HttpResponseData> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "demands/{id}")] HttpRequestData req, string id)
        {
            await _demandService.DeleteAsync(id);

            return req.CreateResponse();
        }

        [Function("Get demand")]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "demands/{id}")] HttpRequestData req, string id)
        {
            var demand = await _demandService.GetAsync(id);

            if (demand == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            return await req.CreateResponseWithContentAsync(demand);
        }

        [Function("Get all demands")]
        public async Task<HttpResponseData> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "demands")] HttpRequestData req)
        {
            var demands = await _demandService.GetAllActiveAsync();

            return await req.CreateResponseWithContentAsync(demands);
        }

        [Function("Update demand")]
        public async Task<HttpResponseData> Update([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "demands/{id}")] HttpRequestData req, string id)
        {
            var updateRequest = await req.GetJsonBody<UpdateDemandRequestDto, UpdateDemandRequestValidator>();
            if (!updateRequest.IsValid)
            {
                return await updateRequest.ToBadRequest(req);
            }

            await _demandService.UpdateAsync(id,
                                             updateRequest.Value.Zone,
                                             updateRequest.Value.Type,
                                             updateRequest.Value.Price);

            return req.CreateResponse();
        }

        [Function("Reactivate demand")]
        public async Task<HttpResponseData> ReactivateTicket([HttpTrigger(AuthorizationLevel.Anonymous, "patch", Route = "demands/{id}/reactivate")] HttpRequestData req, string id)
        {
            await _demandService.ReactivateDemandAsync(id);

            return req.CreateResponse();
        }

        [Function("Deactivate old demands")]
        public async Task Run([TimerTrigger("0 0 4 * * *")] TimerInfo timer)
        {
            await _demandService.DeactivateOldDemandsAsync();
        }
    }
}
