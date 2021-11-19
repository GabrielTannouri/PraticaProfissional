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
    public class OrdemServicoController : Controller
    {
        DAOOrdemServico daoOrdemServico = new DAOOrdemServico();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new ViewModel.OrdemServicoVM
            {
                dtSituacao = DateTime.Now.ToString("yyyy-MM-dd")
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ViewModel.OrdemServicoVM model)
        {
            if (model.flSituacao == "A")
            {
                if (string.IsNullOrEmpty(model.flSituacao))
                {
                    ModelState.AddModelError("flSituacao", "Por favor informe a situação!");
                }

                if (model.Funcionario.idFuncionario == null)
                {
                    if (string.IsNullOrEmpty(model.Funcionario.nmFuncionario))
                    {
                        ModelState.AddModelError("Funcionario.idFuncionario", "Por favor informe o funcionário!");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var dtAtual = DateTime.Today;
                model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
                model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");
                try
                {
                    //Populando o objeto para salvar;
                    var obj = model.VM2E(new Models.OrdemServico());

                    //Instanciando e chamando a DAO para salvar o objeto OS no banco;
                    var daoOrdemServico = new DAOOrdemServico();

                    if (daoOrdemServico.Create(obj))
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
        public ActionResult Edit(int id, ViewModel.OrdemServicoVM model)
        {
            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                try
                {
                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoOrdemServico = new DAOOrdemServico();

                    model.dtAtualizacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");
                    model.dtSituacao = DateTime.Today.ToString("dd/MM/yyyy HH:mm");

                    if (model.flSituacaoAberta != null)
                    {
                        model.flSituacao = model.flSituacaoAberta;
                    }
                    else if (model.flSituacaoAprovado != null)
                    {
                        model.flSituacao = model.flSituacaoAprovado;
                    }
                    else if (model.flSituacaoFechado != null)
                    {
                        model.flSituacao = model.flSituacaoFechado;
                    }
                    else
                    {
                        model.flSituacao = model.flSituacaoRealizado;
                    }

                    //Populando o objeto para alterar;
                    var bean = daoOrdemServico.GetOrdemServicoByID(id);
                    //Populando o objeto para alterar;
                    var obj = model.VM2E(bean);

                    if (daoOrdemServico.Edit(obj))
                    {
                        if (model.flSituacao == "I")
                        {
                            return RedirectToAction("Create", "Venda", new { idOrdemServico = id});
                        }
                        else
                        {
                            TempData["message"] = "Registro alterado com sucesso!";
                            TempData["type"] = "sucesso";
                        }
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
                var daoOrdemServico = new DAOOrdemServico();

                var model = daoOrdemServico.GetOrdemServicoByID(id);

                if (model.CondicaoPagamento.idCondicaoPagamento == 0)
                {
                    model.CondicaoPagamento.idCondicaoPagamento = null;
                }

                var VM = new ViewModel.OrdemServicoVM
                {
                    idOrdemServico = model.idOrdemServico,
                    dtSituacao = model.dtSituacao.ToString("yyyy-MM-dd"),
                    flSituacao = model.flSituacao,
                    flSituacaoAux = daoOrdemServico.Situacao(model.flSituacao),
                    Funcionario = new ViewModel.FuncionarioVM
                    {
                        idFuncionario = model.Funcionario.idPessoa,
                        text = model.Funcionario.nmPessoa,
                    },
                    Cliente = new ViewModel.ClienteVM
                    {
                        idCliente = model.Funcionario.idPessoa,
                        text = model.Funcionario.nmPessoa,
                    },
                    Produto = new ViewModel.ProdutoVM
                    {
                        idProduto = model.Produto.idProduto,
                        text = model.Produto.nmProduto,
                    },
                    CondicaoPagamento = new ViewModel.CondicaoPagamentoVM
                    {
                        idCondicaoPagamento = model.CondicaoPagamento.idCondicaoPagamento != null ? model.CondicaoPagamento.idCondicaoPagamento : null,
                        text = model.CondicaoPagamento.nmCondicaoPagamento,
                    },
                    dsProduto = model.dsProduto,
                    dsProblema = model.dsProblema,
                    ListProdutosItem = model.ItensOrdemServico,
                    ListServicosItem = model.ServicosOrdemServico,
                    ListHistoricoOrdemServico = model.historico,
                    vlDesconto = model.vlDesconto,
                    vlTotal = model.vlTotal,
                    dtCadastro = model.dtCadastro.ToString("dd/MM/yyyy HH:mm"),
                    dtAtualizacao = model.dtAtualizacao.ToString("dd/MM/yyyy HH:mm"),
                };
                ViewBag.flSituacao = model.flSituacao;
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

        public JsonResult JsSearch([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            try
            {

                var daoOrdemServico = new DAOOrdemServico();

                var select = daoOrdemServico.GetOrdemServicos();

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