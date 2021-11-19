using Pratica_Profissional.DAO;
using Pratica_Profissional.Models;
using Pratica_Profissional.Util;
using Pratica_Profissional.Util.DataTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pratica_Profissional.Controllers
{
    public class ContaReceberController : Controller
    {
        DAOContaReceber daoContaReceber = new DAOContaReceber();

        public ActionResult Index()
        {
            var daoContaReceber = new DAOContaReceber();
            List<ContasReceber> lista = daoContaReceber.GetContasReceber().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Pagar(string modNota, string serieNota, int nrNota, int nrParcela, int idCliente)
        {
            return (this.GetView(modNota, serieNota, nrNota, nrParcela, idCliente));
        }

        [HttpPost]
        public ActionResult Pagar(ViewModel.ContaReceberVM model)
        {
            if (model.Conta.idConta == null)
            {
                ModelState.AddModelError("Conta.idConta", "Por favor informe uma conta!");

            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Populando o objeto para salvar;
                    var obj = model.VM2E(new Models.ContasReceber());

                    //Instanciando e chamando a DAO para salvar o objeto OS no banco;
                    var daoContaReceber = new DAOContaReceber();

                    if (daoContaReceber.Pagar(obj))
                    {
                        TempData["message"] = "Registro inserido com sucesso!";
                        TempData["type"] = "sucesso";
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    this.AddFlashMessage(ex.Message, FlashMessage.ERROR);
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(string modNota, string serieNota, int nrNota, int nrParcela, int idCliente)
        {
            return (this.GetView(modNota, serieNota, nrNota, nrParcela, idCliente));
        }

        private ActionResult GetView(string modNota, string serieNota, int nrNota, int nrParcela, int idCliente)
        {
            try
            {
                var daoContaReceber = new DAOContaReceber();

                var model = daoContaReceber.GetContasReceberbyID(modNota, serieNota, nrNota, nrParcela, idCliente);

                var VM = new ViewModel.ContaReceberVM
                {
                    modNota = model.modNota,
                    serieNota = model.serieNota,
                    nrNota = model.nrNota,
                    dtVencimento = model.dtVencimento.ToString("yyyy-MM-dd"),
                    dtPagamento = DateTime.Now.ToString("yyyy-MM-dd"),
                    nrParcela = model.nrParcela.GetValueOrDefault(),
                    vlParcela = model.dtVencimento > DateTime.Now ? model.vlParcela - model.vlDesconto : model.vlParcela + model.vlJuros + model.vlMulta,
                    vlDesconto = model.vlDesconto,
                    vlMulta = model.vlMulta,
                    vlJuros = model.vlJuros,

                    //Cliente = new ViewModel.ClienteVM
                    //{
                    //    idCliente = model.Funcionario.idPessoa,
                    //    text = model.Funcionario.nmPessoa,
                    //},

                };
                return View(VM);
            }
            catch
            {
                return View();
            }
        }

        public JsonResult JsDetails(int? id, string q)
        {
            try
            {
                var result = this.Find(id, q).FirstOrDefault();
                if (result != null)
                    return Json(result, JsonRequestBehavior.AllowGet);
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }


        private IQueryable<dynamic> Find(int? id, string q)
        {
            var daoCondicaoPagamento = new DAOCondicaoPagamento();
            var list = daoCondicaoPagamento.GetCondicaoPagamentos();
            var select = list.Select(u => new
            {
                id = u.idCondicaoPagamento,
                text = u.nmCondicaoPagamento,

            }).OrderBy(u => u.text).ToList();
            if (id != null)
            {
                return select.Where(u => u.id == id).AsQueryable();
            }
            else
            {
                return select.AsQueryable();
            }
        }

        public JsonResult JsSearch([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            try
            {

                var daoContaReceber = new DAOContaReceber();

                var select = daoContaReceber.GetContasReceber();

                var totalResult = select.Count();
                var result = select.OrderBy(requestModel.Columns, requestModel.Start, requestModel.Length).ToList();

                return Json(new DataTablesResponse(requestModel.Draw, result, totalResult, result.Count), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
            }
        }
    }
}