using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleFileUpload.Server.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class UploadController : ControllerBase
	{
		private readonly IWebHostEnvironment _Env;

		// Cheer 100 iwattcher 13/9/19 
		// Cheer 200 goranhal 13/9/19 

		public UploadController(IWebHostEnvironment env)
		{
			_Env = env;
		}

		[HttpPost()]
		public async Task<IActionResult> Post(List<IFormFile> files)
		{
			long size = files.Sum(f => f.Length);

			foreach (var formFile in files)
			{

				// full path to file in temp location
				var filePath = Path.GetTempFileName();

				if (formFile.Length > 0)
				{
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await formFile.CopyToAsync(stream);
					}
				}

				// DON'T DO THIS... Jeff is trusting himself
				System.IO.File.Copy(filePath, Path.Combine(_Env.ContentRootPath, "Uploaded", formFile.FileName));

			}

			// process uploaded files
			// Don't rely on or trust the FileName property without validation.

			return Ok(new { count = files.Count, size });

		}


	}

}