using Microsoft.AspNetCore.Mvc;

namespace rest_file_server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BundlesController : ControllerBase
    {
        private const string dataFolderName = "/var/data";
        private readonly ILogger<BundlesController> _logger;

        public BundlesController(ILogger<BundlesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetFileNames()
        {
            try
            {
                return Ok(await Task.Run(() => Directory.EnumerateFiles(dataFolderName,"*.tar.gz").Select(n => Path.GetFileName(n))));
            }
            catch (Exception x)
            {

                return StatusCode(500, x);
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetSpecific(string name)
        {
            if(name.EndsWith(".tar.gz"))
            {
                name = name[..^".tar.gz".Length];
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("resource name cannot be empty or whitespaces");
            }

            try
            {
                string? filename = await Task.Run(() => Directory.EnumerateFiles(dataFolderName, $"{name}.tar.gz").FirstOrDefault());

                if (string.IsNullOrWhiteSpace(filename))
                {
                    return NotFound(name);
                }

                return PhysicalFile(filename, "application/gzip", $"{name}.tar.gz");
            }
            catch (Exception x)
            {

                return StatusCode(500, x);
            }
        }

        [HttpGet("newest")]
        public async Task<IActionResult> GetNewest()
        {
            try
            {
                DateTime newestDate = DateTime.MinValue;
                string newestFilename = string.Empty;
                foreach(string filename in await Task.Run(() => Directory.EnumerateFiles(dataFolderName, "*.tar.gz")))
                {
                    DateTime lastUpdated = await Task.Run(() => Directory.GetLastWriteTime(filename));
                    if (lastUpdated > newestDate) 
                    {
                        newestFilename = filename;
                        newestDate = lastUpdated;
                    }
                }

                return PhysicalFile(newestFilename, "application/gzip", "bundle.tar.gz");
            }
            catch (Exception x)
            {
                return StatusCode(500, x);
            }
        }
    }
}