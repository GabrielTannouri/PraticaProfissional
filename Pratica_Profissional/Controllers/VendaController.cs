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
    public class VendaController : Controller
    {
        //DAOCompra daoCompra = new DAOCompra();
        // GET: Paises
        public ActionResult Index()
        {
            //var daoCompra = new DAOCompra();
            //List<Compra> lista = daoCompra.GetCondicaoPagamentos().ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Create(int? idOrdemServico)
        {
            var VM = new ViewModel.VendaVM()
            {
                modNota = "55",
                serieNota = "1",
                dtVenda = DateTime.Now.ToString("yyyy-MM-dd"),
            };

            if (idOrdemServico != null)
            {
                var daoOrdemServico = new DAOOrdemServico();
                var objOrdemServico = daoOrdemServico.GetOrdemServicoByID(idOrdemServico ?? 0);

                VM.modNota = "55";
                VM.serieNota = "1";
                VM.dtVenda = DateTime.Now.ToString("yyyy-MM-dd");
                VM.Funcionario = new ViewModel.FuncionarioVM
                {
                    idFuncionario = objOrdemServico.Funcionario.idPessoa,
                    text = objOrdemServico.Funcionario.nmPessoa
                };
                VM.Cliente = new ViewModel.ClienteVM
                {
                    idCliente = objOrdemServico.Cliente.idPessoa,
                    text = objOrdemServico.Cliente.nmPessoa
                };
                return View(VM);
            }

            ViewBag.flOrigem = "";
            return View(VM);
        }

        [HttpPost]
        public ActionResult Create(ViewModel.CondicaoPagamentoVM model)
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

            var dtAtual = DateTime.Today;
            model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
            model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");
            try
            {
                //Populando o objeto para salvar;
                var obj = model.VM2E(new Models.CondicaoPagamento());

                //Instanciando e chamando a DAO para salvar o objeto país no banco;
                var daoCondicaoPagamento = new DAOCondicaoPagamento();

                if (daoCondicaoPagamento.Create(obj))
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
            return (this.GetView(id));
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
        public ActionResult Delete(int id)
        {
            return (this.GetView(id));
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
        public ActionResult Details(int id)
        {
            return (this.GetView(id));
        }

        private ActionResult GetView(int id)
        {
            try
            {
                var daoCondicao = new DAOCondicaoPagamento();

                var model = daoCondicao.GetCondicaoPagamentosByID(id);

                var VM = new ViewModel.CondicaoPagamentoVM
                {
                    idCondicaoPagamento = model.idCondicaoPagamento,
                    nmCondicaoPagamento = model.nmCondicaoPagamento,
                    txJuros = model.txJuros,
                    multa = model.multa,
                    desconto = model.desconto,
                    dtCadastro = model.dtCadastro.ToString("dd/MM/yyyy HH:mm"),
                    dtAtualizacao = model.dtAtualizacao.ToString("dd/MM/yyyy HH:mm"),
                    ListCondicao = model.CondicaoParcelas,
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

        //public JsonResult JsCreate(Pais pais)
        //{
        //    var dtAtual = DateTime.Today;
        //    pais.dtCadastro = dtAtual;
        //    pais.dtAtualizacao = dtAtual;
        //    try
        //    {
        //        var daoPaises = new DAOPais();
        //        daoPaises.Create(pais);
        //        var result = new
        //        {
        //            type = "success",
        //            message = "País adicionado",
        //            model = pais
        //        };
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 500;
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult JsUpdate(Pais model)
        //{
        //    var daoPaises = new DAOPais();
        //    daoPaises.Edit(model);
        //    var result = new
        //    {
        //        type = "success",
        //        field = "",
        //        message = "Registro alterado com sucesso!",
        //        model = model
        //    };
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

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