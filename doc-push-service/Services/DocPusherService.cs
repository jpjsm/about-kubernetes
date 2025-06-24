using doc_push_service.BackgroundServices;
using Microsoft.Extensions.Logging.Abstractions;

namespace doc_push_service.Services
{
    public class DocPusherService
    {
        private readonly ILogger<DocPusherService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;


        public readonly string packageLocation = "/var/push-service/packages";

        public DocPusherService(ILogger<DocPusherService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger ?? NullLogger<DocPusherService>.Instance;
            _httpClientFactory = httpClientFactory; 
        }

        public async Task PushDocumentsAsync()
        {
            // Package 
            // --> A package is a compressed archive formed by a Bill-of-Lading (BOL) document
            //     and a folder structure containing policies and data
            //
            // --> BOL structure
            //        [ {"path": "(data|policies)/<path-spec>", "operation": "(create-if-not-exists|create-or-overwrite|delete|patch)", "arguments": "<argument-values>"}, ... ]
            //
            //        path:       [Mandatory] A valid path to a document in the archive
            //        operation:  [Mandatory] An enumeration formed by one of the following values: create-if-not-exists | create-or-overwrite | delete | patch
            //        arguments:  [optional] The arguments to the operation, if the operation requires arguments.
            //     
            // --> archive structure
            //        bol.json
            //        policies/
            //           <policy-name>
            //           ...
            //        data/
            //           <document>
            //           <path>/<sub-path>/.../<document>
            //           ...

            // Get the list of packages available to push
            List<Tuple<(string filename, DateTime lastUpdated)>> packageList = new();
            foreach(string filename in await Task.Run(() => Directory.EnumerateFiles(packageLocation, "*.tar.gz")))
            {
                DateTime lastUpdated = await Task.Run(() => Directory.GetLastWriteTime(filename));
                packageList.Add(new Tuple<(string filename, DateTime lastUpdated)>((filename, lastUpdated)));
            }
         
            _logger.LogInformation($"Total packages found: {packageList.Count}");
            if (packageList.Count == 0)
            {
                _logger.LogInformation($"Nothing to do, there are no packages to process.");
                return;
            }
            packageList = packageList.OrderByDescending(x => x.Item1.lastUpdated).ToList();


            // Get the list of endpoints to push to
            // --> The list contains endpoint and last version pushed


            // for each endpoint push all packages not deployed yet


            Console.WriteLine("[DocPusherService.PushDocumentsAsync] Beginning");
            await Task.Delay(100);
            _logger.LogInformation(
                "Sample Service did something.");

        }

    }
}
