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
    public class FormaPagamentoController : Controller
    {
        DAOFormaPagamento dAOFormaPagamento = new DAOFormaPagamento();
        //GET: Estados
        public ActionResult Index()
        {
            var daoFormaPagamento = new DAOFormaPagamento();
            List<FormaPagamento> lista = daoFormaPagamento.GetFormaPagamentos().ToList();
            return View(lista);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewModel.FormaPagamentoVM model)
        {
            if (string.IsNullOrEmpty(model.nmFormaPagamento))
            {
                ModelState.AddModelError("nmFormaPagamento", "Por favor informe a forma de pagamento!");
            }

            if (model.nmFormaPagamento != null)
            {
                if (string.IsNullOrEmpty(model.nmFormaPagamento.Trim()))
                {
                    ModelState.AddModelError("nmFormaPagamento", "Por favor informe o nome da forma de pagamento!");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Setando as datas atuais;
                    var dtAtual = DateTime.Today;
                    model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
                    model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para salvar;
                    var obj = model.VM2E(new Models.FormaPagamento());

                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoformaPagamentos = new DAOFormaPagamento();

                    if (daoformaPagamentos.Create(obj))
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
        public ActionResult Edit(int id)
        {
            return (this.GetView(id));
        }

        [HttpPost]
        public ActionResult Edit(int id, ViewModel.FormaPagamentoVM model)
        {
            if (string.IsNullOrEmpty(model.nmFormaPagamento))
            {
                ModelState.AddModelError("nmformaPagamento", "Por favor informe o nome do país!");
            }

            if (model.nmFormaPagamento != null)
            {
                if (string.IsNullOrEmpty(model.nmFormaPagamento.Trim()))
                {
                    ModelState.AddModelError("nmFormaPagamento", "Por favor informe o nome da forma de pagamento!");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoformaPagamentos = new DAOFormaPagamento();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    //Populando o objeto para alterar;
                    var bean = daoformaPagamentos.GetFormaPagamentosByID(id);
                    var obj = model.VM2E(bean);


                    if (daoformaPagamentos.Edit(obj))
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
                var daoFormaPagamentos = new DAOFormaPagamento();

                if (daoFormaPagamentos.Delete(id))
                {
                    TempData["message"] = "Registro excluído com sucesso!";
                    TempData["type"] = "sucesso";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["message"] = "O registro não pode ser excluído, pois está associado a um estado!";
                TempData["type"] = "erro";
                return RedirectToAction("Index");
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
                var daoFormaPagamentos = new DAOFormaPagamento();

                var model = daoFormaPagamentos.GetFormaPagamentosByID(id);

                var VM = new ViewModel.FormaPagamentoVM
                {
                    idFormaPagamento = model.idFormaPagamento,
                    nmFormaPagamento = model.nmFormaPagamento,
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

        public JsonResult JsCreate(FormaPagamento formaPagamento)
        {
            var dtAtual = DateTime.Today;
            formaPagamento.dtCadastro = dtAtual;
            formaPagamento.dtAtualizacao = dtAtual;
            try
            {
                var daoFormaPagamento = new DAOFormaPagamento();
                daoFormaPagamento.Create(formaPagamento);
                var result = new
                {
                    type = "success",
                    message = "Forma de pagamento adicionado",
                    model = formaPagamento
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult JsUpdate(FormaPagamento model)
        {
            var daoFormaPagamento = new DAOFormaPagamento();
            daoFormaPagamento.Edit(model);
            var result = new
            {
                type = "success",
                field = "",
                message = "Registro alterado com sucesso!",
                model = model
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IQueryable<dynamic> Find(int? id, string q)
        {
            var daoFormaPagamento = new DAOFormaPagamento();
            var list = daoFormaPagamento.GetFormaPagamentos();
            var select = list.Select(u => new
            {
                id = u.idFormaPagamento,
                text = u.nmFormaPagamento,
                dtCadastro = u.dtCadastro,
                dtUltAlteracao = u.dtAtualizacao
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
