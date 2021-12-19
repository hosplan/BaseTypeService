using BaseTypeService.Data;
using BaseTypeService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseTypeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseRootTypeController : ControllerBase
    {
        private readonly BaseTypeContext _context;

        public BaseRootTypeController(BaseTypeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Load()
        {            
            try
            {                
                return Ok(new { token = true , data = _context.BaseBranchType.ToList() });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false , data = "에러가 발생했습니다." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] BaseRootType baseRootType)
        {
            try
            {
                _context.Add(baseRootType);
                await _context.SaveChangesAsync();

                return Ok(new { token = true });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false });
            }
        }
        [HttpPatch]
        public async Task<IActionResult> Update([FromForm] BaseRootType baseRootType)
        {
            try
            {
                var updateValue = _context.BaseRootType.FirstOrDefault(b => b.Id == baseRootType.Id);

                if(updateValue == null) { return Ok(new { token = false }); }

                updateValue.Name = baseRootType.Name;
                updateValue.Description = baseRootType.Description;
                updateValue.Color = baseRootType.Color;

                _context.Update(updateValue);
                await _context.SaveChangesAsync();

                return Ok(new { token = false });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false });
            }
        }
    }
}
