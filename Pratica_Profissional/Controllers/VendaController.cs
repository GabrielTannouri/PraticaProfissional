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
        DAOVenda DAOVenda = new DAOVenda();
        // GET: Paises
        public ActionResult Index()
        {
            var daoVenda = new DAOVenda();
            List<Venda> lista = daoVenda.GetVendas().ToList();
            return View(lista);
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
                var objItensOrdemServico = daoOrdemServico.GetItensByID(idOrdemServico ?? 0);
                var objServicosOrdemServico = daoOrdemServico.GetServicosOrdemServico(idOrdemServico ?? 0);

                var listItens = new List<ItemVenda>();
                foreach (var item in objItensOrdemServico)
                {
                    var itens = new Models.ItemVenda
                    {
                        idProduto = item.idProduto,
                        nmProduto = item.nmProduto,
                        flUnidade = item.flUnidade,
                        qtProduto = item.qtProduto,
                        vlUnitario = item.vlUnitario,
                        vlTotalProduto = item.vlTotalProduto,
                    };
                    listItens.Add(itens);
                }

                var listServicos = new List<ServicosVenda>();
                foreach (var item in objServicosOrdemServico)
                {
                    var itens = new Models.ServicosVenda
                    {
                        idServico = item.idServico,
                        nmServico = item.nmServico,
                        nmFuncionario = item.nmFuncionario,
                        qtServico = item.qtServico,
                        vlUnitarioServico = item.vlUnitarioServico,
                        vlTotalServico = item.vlTotalServico,
                    };
                    listServicos.Add(itens);
                }

                VM.modNota = "55";
                VM.modNotaServico = "56";
                VM.serieNota = "1";
                VM.serieNotaServico = "1";
                VM.dtVenda = DateTime.Now.ToString("yyyy-MM-dd");
                VM.dtVendaServico = DateTime.Now.ToString("yyyy-MM-dd");
                VM.Funcionario = new ViewModel.FuncionarioVM
                {
                    idFuncionario = objOrdemServico.Funcionario.idPessoa,
                    text = objOrdemServico.Funcionario.nmPessoa
                };
                VM.FuncionarioServico = new ViewModel.FuncionarioVM
                {
                    idFuncionario = objOrdemServico.Funcionario.idPessoa,
                    text = objOrdemServico.Funcionario.nmPessoa
                };
                VM.Cliente = new ViewModel.ClienteVM
                {
                    idCliente = objOrdemServico.Cliente.idPessoa,
                    text = objOrdemServico.Cliente.nmPessoa
                };
                VM.ClienteServico = new ViewModel.ClienteVM
                {
                    idCliente = objOrdemServico.Cliente.idPessoa,
                    text = objOrdemServico.Cliente.nmPessoa
                };
                VM.ListItemVenda = listItens;
                VM.ListServicosVenda = listServicos;
                VM.CondicaoPagamento = new ViewModel.CondicaoPagamentoVM
                {
                    idCondicaoPagamento = objOrdemServico.CondicaoPagamento.idCondicaoPagamento,
                    text = objOrdemServico.CondicaoPagamento.nmCondicaoPagamento,
                };
                VM.CondicaoPagamentoServico = new ViewModel.CondicaoPagamentoVM
                {
                    idCondicaoPagamento = objOrdemServico.CondicaoPagamento.idCondicaoPagamento,
                    text = objOrdemServico.CondicaoPagamento.nmCondicaoPagamento,
                };
                ViewBag.flOrigem = "ordemServico";
                return View(VM);
            }

            ViewBag.flOrigem = "";
            return View(VM);
        }

        [HttpPost]
        public ActionResult Create(ViewModel.VendaVM model)
        {
            if (string.IsNullOrEmpty(model.modNota))
            {
                ModelState.AddModelError("modNota", "Por favor informe o modelo da nota!");
            }

            if (model.serieNota != null)
            {
                if (string.IsNullOrEmpty(model.serieNota.Trim()))
                {
                    ModelState.AddModelError("serieNota", "Por favor informe a série da nota!");
                }
            }

            if (model.Funcionario.idFuncionario == null)
            {
                ModelState.AddModelError("Funcionario.idFuncionario", "Por favor informe o funcionário!");
            }

            if (model.Cliente.idCliente == null)
            {
                ModelState.AddModelError("Cliente.idCliente", "Por favor informe o cliente!");
            }

            if (string.IsNullOrEmpty(model.dtVenda))
            {
                ModelState.AddModelError("dtVenda", "Por favor informe a data da venda!");
            }

            if (model.ListItemVenda.Count() == 0 && model.ListVendaParcelas.Count() == 0)
            {
                ModelState.AddModelError("ListItemVenda", "Por favor informe ao menos um produto!");
                ModelState.AddModelError("ListVendaParcelas", "Por favor informe ao menos um serviço ou produto!");
            }

            if (ModelState.IsValid)
            {
                var dtAtual = DateTime.Today;
                model.dtCadastro = dtAtual.ToString("dd/MM/yyyy HH:mm");
                model.dtAtualizacao = dtAtual.ToString("dd/MM/yyyy HH:mm");
                try
                {
                    //Populando o objeto para salvar;
                    var obj = model.VM2E(new Models.Venda());

                    //Instanciando e chamando a DAO para salvar o objeto país no banco;
                    var daoVenda = new DAOVenda();
                    var daoCondicaoPagamento = new DAOCondicaoPagamento();
                    obj.flSituacao = "A";
                    if (daoVenda.Create(obj, daoCondicaoPagamento.GetCondicaoPagamentosByID(obj.idCondPagamento)))
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

                var daoVenda = new DAOVenda();

                var select = daoVenda.GetVendas();

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