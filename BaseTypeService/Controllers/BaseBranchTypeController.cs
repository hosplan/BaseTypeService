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
    [Route("/base_branch_type")]
    [ApiController]
    public class BaseBranchTypeController : ControllerBase
    {
        private readonly BaseTypeContext _context;

        public BaseBranchTypeController(BaseTypeContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetBaseBranchType(int id)
        {
            try
            {
                BaseBranchType baseBranchType = _context.BaseBranchType.FirstOrDefault(b => b.Id == id);
                return Ok(new { token = true, data = baseBranchType });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false, data = "오류가 발생 했습니다." });
            }
        }
        
        [HttpGet("root_type={id}")]
        public IActionResult Load(int id)
        {
            try
            {
                return Ok(new { token = true, data = _context.BaseBranchType.AsNoTracking().Where(b => b.BaseRootTypeId == id).ToList() });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false, data = "오류가 발생 했어요.." });
            }
        }

        [HttpGet]
        public IActionResult Load()
        {
            try
            {
                List<BaseRootType> rootTypes = _context.BaseRootType.AsNoTracking().ToList();
                List<BaseBranchType> branchTypes = _context.BaseBranchType.AsNoTracking().ToList();

                return Ok(new { token = true, branchTypes = branchTypes, rootTypes = rootTypes });
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false, data = "오류가 발생 했어요.." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] BaseBranchType baseBranchType)
        {
            try
            {
                if(CheckDuplicationName(baseBranchType) == false)
                {
                    return Ok(new { token = false , data = "이름이 중복 되었어요."}); 
                }
                
                _context.Add(baseBranchType);
                await _context.SaveChangesAsync();

                return Ok(new { token = true, data = baseBranchType });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false , data = "오류가 발생 했어요.. 잠시 후에 다시 시도해주세요." });
            }
        }

        
        [HttpPatch]
        public async Task<IActionResult> Update([FromForm] BaseBranchType baseBranchType)
        {
            try
            {

                BaseBranchType updateValue = _context.BaseBranchType.FirstOrDefault(b => b.Id == baseBranchType.Id);

                if (updateValue == null) { return Ok(new { token = false }); }

                updateValue.Name = baseBranchType.Name;
                updateValue.Description = baseBranchType.Description;
                updateValue.Color = baseBranchType.Color;

                _context.Update(updateValue);
                await _context.SaveChangesAsync();

                return Ok(new { token = true , data = updateValue });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] BaseBranchType removeValue)
        {
            try
            {
                BaseBranchType baseBranchType = _context.BaseBranchType.FirstOrDefault(b => b.Id == removeValue.Id);
                int id = baseBranchType.Id;
                _context.Remove(baseBranchType);
                await _context.SaveChangesAsync();

                return Ok(new { token = true, data = id });
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return Ok(new { token = false });
            }
        }

        /// <summary>
        /// 타입 이름 중복검사
        /// </summary>
        /// <param name="baseBranchType"></param>
        /// <returns></returns>
        private bool CheckDuplicationName(BaseBranchType baseBranchType)
        {
            try
            {
                if(_context.BaseBranchType.FirstOrDefault(b => b.BaseRootTypeId == baseBranchType.BaseRootTypeId
                                                                                && b.Name == baseBranchType.Name) != null)
                {
                    return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return false;
            }
        }
    }
}
