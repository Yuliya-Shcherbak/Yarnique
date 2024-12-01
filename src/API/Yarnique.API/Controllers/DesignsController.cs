using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Yarnique.API.Modules.Designs.DesignParts;
using Yarnique.API.Modules.Designs.Designs;
using Yarnique.Common.Application.Pagination;
using Yarnique.Modules.Designs.Application.Contracts;
using Yarnique.Modules.Designs.Application.DesignCreation.CreateDesign;
using Yarnique.Modules.Designs.Application.DesignCreation.CreateDesignPart;
using Yarnique.Modules.Designs.Application.DesignCreation.EditDesign;
using Yarnique.Modules.Designs.Application.DesignCreation.GetAllDesignParts;
using Yarnique.Modules.Designs.Application.DesignCreation.GetDesign;
using Yarnique.Modules.Designs.Application.DesignCreation.GetDesignPartPatternPreview;
using Yarnique.Modules.Designs.Application.DesignCreation.PublishDesign;
using Yarnique.Modules.Designs.Application.DesignCreation.UploadDesignPartPattern;

namespace Yarnique.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/designs")]
    public class DesignsController : BaseController
    {
        private readonly IDesignsModule _designsModule;

        public DesignsController(IDesignsModule designsModule)
        {
            _designsModule = designsModule;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<DesignPartDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDesignById([FromQuery] GetDesignRequest request)
        {
            var result = await _designsModule.ExecuteQueryAsync(new GetDesignQuery(request.Id));

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateDesign([FromBody] CreateDesignRequest request)
        {
            var designId = await _designsModule.ExecuteCommandAsync(
                new CreateDesignCommand(
                    request.Name,
                    request.Price,
                    _userId(),
                    request.Parts.Select(x => new CreateDesignPartSpecificationCommand(x.DesignPartId, x.YarnAmount, x.Order, x.Term)).ToList()
                    )
                );

            return Ok(designId);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDesign([FromRoute] Guid id, [FromBody] EditDesignRequest request)
        {
            await _designsModule.ExecuteCommandAsync(
                new EditDesignCommand(
                    id,
                    request.Name,
                    request.Price,
                    request.Parts.Select(x => new CreateDesignPartSpecificationCommand(x.DesignPartId, x.YarnAmount, x.Order, x.Term)).ToList()
                    )
                );

            return Ok();
        }

        [HttpPut("{id}/publish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PublishDesign([FromRoute] Guid id)
        {
            await _designsModule.ExecuteCommandAsync(new PublishDesignCommand(id));

            return Ok();
        }

        [HttpGet("parts")]
        [ProducesResponseType(typeof(List<DesignPartDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllDesignParts([FromQuery] PaginatedRequest request)
        {
            var result = await _designsModule.ExecuteQueryAsync(new GetAllDesignPartsQuery(request.PageNumber, request.PageSize));

            return Ok(result);
        }

        [HttpPost("parts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateDesignPart([FromBody] CreateDesignPartRequest request)
        {
            await _designsModule.ExecuteCommandAsync(new CreateDesignPartCommand(request.Name));

            return Ok();
        }

        [HttpPost("parts/{id}/upload")]
        public async Task<IActionResult> UploadImage([FromRoute] Guid id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            
            await _designsModule.ExecuteCommandAsync(new UploadDesignPartPatternCommand(id, file));

            return Ok();
        }

        [HttpGet("parts/{id}/preview")]
        [Produces(MediaTypeNames.Image.Jpeg, Type = typeof(File))]
        public async Task<IActionResult> GetDesignPartPreview([FromRoute] Guid id)
        {
            var previewImage = await _designsModule.ExecuteQueryAsync(new GetDesignPartPatternPreviewQuery(id));

            return File(previewImage, MediaTypeNames.Image.Jpeg);
        }
    }
}
