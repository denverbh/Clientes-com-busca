using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspnet_Upload1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aspnet_Upload1.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ClientesController(AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            dbContext = context;
            webHostEnvironment = hostEnvironment;
        }
//Criando a ordenação
//Fonte: https://docs.microsoft.com/pt-br/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-6.0
//sortOrder serve para ordenar alguns itens, já searchString é busca.

        public async Task<IActionResult> Index(string sortOrder, string searchString)

        {
            ViewData["NomeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nom_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["CurrentFilter"] = searchString;

            var clientes = from s in dbContext.Clientes
                           select s;
//parte da busca
            if (!String.IsNullOrEmpty(searchString))
            {
                clientes = clientes.Where(s => s.Nome.Contains(searchString)
                                       || s.Email.Contains(searchString));
            }
//parte da ordenação
            switch (sortOrder)
            {
                case "nome_desc":
                    clientes = clientes.OrderByDescending(s => s.Nome);
                    break;
                case "Email":
                    clientes = clientes.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    clientes = clientes.OrderByDescending(s => s.Email);
                    break;
                default:
                    clientes = clientes.OrderBy(s => s.Nome);
                    break;
            }
            return View(await clientes.AsNoTracking().ToListAsync());
        }

        //public async Task<IActionResult> Index()
        //{
            //var cliente = await dbContext.Clientes.ToListAsync();
            //return View(cliente);
        //}
        


        public IActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

//Como anexar uma foto 
//fonte: https://www.macoratti.net/20/03/aspc_imguplod1.htm
//https://www.macoratti.net/20/03/aspc_imguplod2.htm

        public async Task<IActionResult> Novo(ClienteViewModel model)
        {
            if (ModelState.IsValid)
            {
                string nomeUnicoArquivo = UploadedFile(model);

                Cliente employee = new Cliente
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Idade = model.Idade,
                    Foto = nomeUnicoArquivo,
                };
                dbContext.Add(employee);
                await dbContext.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        private string UploadedFile(ClienteViewModel model)
        {
            string nomeUnicoArquivo = null;

            if (model.Foto != null)
            {
                string pastaFotos = Path.Combine(webHostEnvironment.WebRootPath, "Imagens");
                nomeUnicoArquivo = Guid.NewGuid().ToString() + "_" + model.Foto.FileName;
                string caminhoArquivo = Path.Combine(pastaFotos, nomeUnicoArquivo);
                using (var fileStream = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    model.Foto.CopyTo(fileStream);
                }
            }
            return nomeUnicoArquivo;
        }
    }
}
