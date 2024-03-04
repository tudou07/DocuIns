using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DocuIns.Models.Documents;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.Runtime;

namespace DocuIns.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly UsersContext _context;

        public DocumentsController(UsersContext context)
        {
            _context = context;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var usersContext = _context.Documents.Include(d => d.User);
            return View(await usersContext.ToListAsync());
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Path,UserId,Status,Tag,CreatedDate,ModifiedDate")] Document document, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var credentials = new BasicAWSCredentials("AKIA2MBFTIOOQBGU2YW3", 
                "xaQVhDfv0VY2JBEuvB29LAAyW1MByc8n6zL3Hi4o");
                var region = Amazon.RegionEndpoint.USWest2;
                using (var amazonS3Client = new AmazonS3Client(credentials, region)) {
                    using (var memoryStream = new MemoryStream()) {
                        file.CopyTo(memoryStream);
                        var request = new TransferUtilityUploadRequest {
                            InputStream = memoryStream,
                            Key = file.FileName,
                            BucketName = "docuins-bucket",
                           ContentType = file.ContentType,
                        };

                        var transferUtility = new TransferUtility(amazonS3Client);
                        await transferUtility.UploadAsync(request);
                    }
                }
                document.Path = $"https://d1bj1bc7hxixa3.cloudfront.net/{file.FileName}";
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", document.UserId);
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", document.UserId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Path,UserId,Status,Tag,CreatedDate,ModifiedDate")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", document.UserId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Documents == null)
            {
                return Problem("Entity set 'UsersContext.Documents'  is null.");
            }
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return (_context.Documents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
                // if (pdfFile != null && pdfFile.Length > 0)
                // {
                //     var awsAccessKeyId = "AKIA2MBFTIOO7EIHIATA";
                //     var awsSecretAccessKey = "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";
                //     var awsRegion = Amazon.RegionEndpoint.USWest2;

                //     using (var memoryStream = new MemoryStream())
                //     {
                //         await pdfFile.CopyToAsync(memoryStream);
                //         var s3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, awsRegion);
                //         var fileTransferUtility = new TransferUtility(s3Client);

                //         var key = Guid.NewGuid().ToString(); // Unique key for S3 object
                //         var bucketName = "practicebucket-sahil";

                //         await fileTransferUtility.UploadAsync("Lab2_D6_Amazon.pdf", bucketName, key);

                //         // Set the S3 URL in the document's Path property
                //         document.Path = $"https://{bucketName}.s3.amazonaws.com/{key}";
                //         Console.WriteLine($"Uploaded to: {document.Path}");
                //     }
                // }