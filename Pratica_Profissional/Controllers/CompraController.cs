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
    public class CompraController : Controller
    {
        DAOCompra daoCompra = new DAOCompra();
        // GET: Paises
        public ActionResult Index()
        {
            var daoCompra = new DAOCompra();
            List<Compra> lista = daoCompra.GetCompras().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewModel.CompraVM model)
        {
            decimal vlTotalProdutos = 0;
            var dtAtual = DateTime.Today;
            model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
            model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");
            try
            {
                //Populando o objeto para salvar;
                var obj = model.VM2E(new Models.Compra());

                //Instanciando e chamando a DAO para salvar o objeto país no banco;
                var daoCompra = new DAOCompra();

                foreach (var item in obj.ItensCompra)
                {
                    vlTotalProdutos = vlTotalProdutos + (item.qtProduto * item.vlUnitario);
                }

                if (daoCompra.Create(obj, vlTotalProdutos))
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View ();
        }

        [HttpPost]
        public ActionResult Edit(int id, ViewModel.CondicaoPagamentoVM model)
        {
            if (string.IsNullOrEmpty(model.nmCondicaoPagamento))
            {
                ModelState.AddModelError("nmCondicaoPagamento", "Por favor informe a condição de pagamento!");
            }

            if (model.nmCondicaoPagamento != null)
            {
                if (string.IsNullOrEmpty(model.nmCondicaoPagamento.Trim()))
                {
                    ModelState.AddModelError("nmCondicaoPagamento", "Por favor informe a condição de pagamento!");
                }
            }

            if (model.ListCondicao.Count() == 0)
            {
                ModelState.AddModelError("ListCondicao", "Por favor informe ao menos uma parcela!");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoCondicao = new DAOCondicaoPagamento();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para alterar;
                    var bean = daoCondicao.GetCondicaoPagamentosByID(id);
                    //Populando o objeto para alterar;
                    var obj = model.VM2E(bean);

                    if (daoCondicao.Edit(obj))
                    {
                        TempData["message"] = "Registro alterado com sucesso!";
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
        public ActionResult Cancelar(string modNota, string serieNota, int nrNota, int idFornecedor)
        {
            return (this.GetView(modNota, serieNota, nrNota, idFornecedor));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var daoPaises = new DAOPais();

                if (daoPaises.Delete(id))
                {
                    ViewBag.AlertMsg = "País excluído com sucesso.";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Details(string modNota, string serieNota, int nrNota, int idFornecedor)
        {
            return (this.GetView(modNota, serieNota, nrNota, idFornecedor));
        }

        private ActionResult GetView(string modNota, string serieNota, int nrNota, int idFornecedor)
        {
            try
            {
                var daoCompra = new DAOCompra();

                var model = daoCompra.GetComprasByID(modNota, serieNota, nrNota, idFornecedor);

                var VM = new ViewModel.CompraVM
                {
                    modNota = model.modNota,
                    serieNota = model.serieNota,
                    nrNota = model.nrNota,
                    dtEmissao = model.dtEmissao.ToString("yyyy-MM-dd"),
                    dtEntrega = model.dtEntrega.HasValue ? model.dtEntrega.Value.ToString("yyyy-MM-dd") : string.Empty,
                    vlDespesas = model.vlDespesas,
                    vlFrete = model.vlFrete,
                    vlSeguro = model.vlSeguro,
                    vlTotal = model.vlTotal,
                    ListItemCompra = model.ItensCompra,
                    ListCompraParcelas = model.ContasPagar,
                    Fornecedor = new ViewModel.FornecedorVM
                    {
                        idFornecedor = model.Fornecedor.idPessoa,
                        text = model.Fornecedor.nmPessoa
                    },
                    dtCadastro = model.dtCadastro.ToString("dd/MM/yyyy"),
                    dtAtualizacao = model.dtAtualizacao.ToString("dd/MM/yyyy"),
                };

                return View(VM);
            }
            catch
            {
                return View();
            }
        }

        public JsonResult jsVerificarCompra(string modNota, string serieNota, string nrNota, int idFornecedor)
        {
            try
            {
                var daoCompra = new DAOCompra();
                var retorno = daoCompra.VerificaDuplicidade(modNota, serieNota, nrNota, idFornecedor);
                return Json(retorno, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                throw new Exception(ex.Message);
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

        public JsonResult JsQuery([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {

            try
            {
                var select = this.Find(null, requestModel.Search.Value);

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