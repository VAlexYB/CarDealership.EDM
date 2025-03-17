using CarDealership.EDM.Core.Abstractions.Services;
using CarDealership.EDM.Core.Models;
using CarDealership.EDM.Endpoints.Contracts.Requests;
using CarDealership.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CarDealership.EDM.Endpoints.Api
{
    [ApiController]
    [Route("api/receipts")]
    public class ReceiptsController : ControllerBase
    {
        private readonly IDocumentsService _documentsService;

        public ReceiptsController(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDocumentRequest request)
        {
            var path = await _documentsService.CreateDocument(request.Entity, (DocumentType)request.DocumentType, (DocumentFormat)request.DocumentFormat, request.EntityId);
            return Ok(new {Path = path});
        }
    }
}
