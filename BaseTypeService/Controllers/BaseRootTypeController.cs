using BaseTypeService.Data;
using BaseTypeService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseTypeService.Controllers
{
    [Route("/base_root_type")]
    [ApiController]
    public class BaseRootTypeController : ControllerBase
    {
        private readonly BaseTypeContext _context;

        public BaseRootTypeController(BaseTypeContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetBaseRootType(int id)
        {
            try
            {
                BaseRootType baseRootType = _context.BaseRootType.FirstOrDefault(b => b.Id == id);
                return Ok(new { token = true, data = baseRootType });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false, data = "오류가 발생 했습니다." });
            }                       
        }

        [HttpGet]
        public IActionResult Load()
        {            
            try
            {                
                return Ok(new { token = true , data = _context.BaseRootType.ToList() });
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
                if (CheckDuplicationName(baseRootType) == false)
                {
                    return Ok(new { token = false, data = "이름이 중복 되었어요." });
                }

                _context.Add(baseRootType);
                await _context.SaveChangesAsync();

                return Ok(new { token = true, data = baseRootType });
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

                _context.Update(updateValue);
                await _context.SaveChangesAsync();

                return Ok(new { token = true, data = updateValue });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] BaseRootType removeValue)
        {
            try
            {
                BaseRootType baseRootType = _context.BaseRootType
                                                    .Include(b => b.BaseBrachTypes)
                                                    .FirstOrDefault(b => b.Id == removeValue.Id);
                int id = baseRootType.Id;
                //BaseBrachType 삭제
                _context.RemoveRange(baseRootType.BaseBrachTypes);
                await _context.SaveChangesAsync();

                //BaseRootType 삭제
                _context.Remove(baseRootType);
                await _context.SaveChangesAsync();
                
                return Ok(new { token = true , data = id });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false });
            }
        }

        /// <summary>
        /// 타입 이름 중복검사
        /// </summary>
        /// <param name="baseRootType"></param>
        /// <returns></returns>
        private bool CheckDuplicationName(BaseRootType baseRootType)
        {
            try
            {
                if (_context.BaseRootType.FirstOrDefault(b => b.Name == baseRootType.Name) != null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return false;
            }
        }
    }
}
