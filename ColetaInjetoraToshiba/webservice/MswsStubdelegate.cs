using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace ColetaInjetoraToshiba.webservice
{
    class MswsStubdelegate
    {
        private int _TIMEOUT_WEBMETHOD = 360;
        private static MswsStubdelegate instancia = null;
        private Logger log = LogManager.GetCurrentClassLogger();
        private Logger logInfo = LogManager.GetLogger("msws");
        private msws.msws msws = new msws.msws();
        private idwws.idwws idw = new idwws.idwws();

        public idwws.idwws getIdwWs()
        {
            if (this.idw == null)
            {
                idw = new idwws.idwws();
                //idw.Url = assemblyWebServiceAddress();
                idw.Timeout = (this._TIMEOUT_WEBMETHOD * 1000);
                idw.Proxy = new System.Net.WebProxy(ColetaInjetoraToshiba.Properties.Settings.Default.ColetaInjetoraHaitian_localhost_idwws, true);
            }
            return this.idw;
        }

        public msws.msws getMsWs()
        {
            if (this.msws == null)
            {
                msws = new msws.msws();
                //msws.Url = assemblyWebServiceAddress();
                msws.Timeout = (this._TIMEOUT_WEBMETHOD * 1000);
                msws.Proxy = new System.Net.WebProxy(ColetaInjetoraToshiba.Properties.Settings.Default.ColetaInjetoraHaitian_msws_msws, true);
            }
            return this.msws;
        }


        private MswsStubdelegate()
        {
            log.Debug("IdwwsStubdelegate");
            /*
            idw.Proxy = new System.Net.WebProxy(ColetaFujiFlex.Properties.Settings.Default.ColetaFujiFlex_idwws_idwws, true);
            msws.Proxy = new System.Net.WebProxy(ColetaFujiFlex.Properties.Settings.Default.ColetaFujiFlex_msws_msws, true);
             */
        }

        public static MswsStubdelegate getInstancia()
        {
            if (instancia == null)
            {
                instancia = new MswsStubdelegate();
            }
            return instancia;
        }

        public void icHeartBeat(int idLog, String urlIC, ColetaInjetoraToshiba.idwws.eventoDTO evento)
        {
            try
            {
                getIdwWs().icHeartBeat(idLog, urlIC, evento);
            }
            catch (Exception e)
            {
                log.Error("Falha ao chamar icHeartBeat " + e);
            }

        }
        public msws.icDTO pesquisarIcDTOPorUrlConexao(String urlConexao)
        {

            log.Debug("pesqusiarIcDTOPorUrlConexao para " + urlConexao);

            msws.icDTO retorno = null;
            //retorno.cd_ic = "";
           // msws.msMs teste = null;
                         
            try
            {
                //teste = getMsWs().pesquisarMsMsPorURLConexaoComParametro(urlConexao);
                log.Debug("chamando webservice para " + urlConexao);
                retorno = getMsWs().pesquisarMsIcPorUrlConexao(urlConexao);
            }
            catch (Exception e)
            {
                if (retorno != null)
                {
                    log.Debug("pesquisarIcDTOPorUrlConexao - Retornou informação do DTO");
                }
                else
                {
                    log.Debug("pesquisarIcDTOPorUrlConexao - DTO Está nulo");
                }
                log.Error("pesquisarIcDTOPorUrlConexao para " + urlConexao + " com ERRO " + e.Message + " servidor " + msws.Url);
            }
            return retorno;
        }

        public idwws.cicloDTO getCicloTimeoutEPadrao(String nomePrograma, String maquina)
        {
            log.Debug("getCicloTimeoutEPadrao para " + nomePrograma + " maquina " + maquina);
            idwws.cicloDTO retorno = null;

            if (nomePrograma != "" || nomePrograma != null)
            {
                try
                {
                    log.Debug("chamando webservice em getFolhaCicloTimeOut");
                    retorno = getIdwWs().getCicloTimeoutEPadrao(nomePrograma, maquina);
                    log.Debug("getCicloTimeoutEPadrao - Retornou informação de cicloDTO com cicloPadrao " + retorno.cicloPadrao + "s");
                    log.Debug("getCicloTimeoutEPadrao - Retornou informação de cicloDTO com eficienciaDeCiclo " + retorno.eficienciaCiclo + "%");
                }
                catch (Exception e)
                {
                    if (retorno != null)
                    {
                        log.Debug("getCicloTimeoutEPadrao - Retornou informação de cicloDTO com " + retorno.cicloPadrao + "s");
                    }
                    else
                    {
                        log.Debug("getCicloTimeoutEPadrao - cicloDTO Está nulo");
                    }
                    log.Error("getCicloTimeoutEPadrao para " + nomePrograma + " com ERRO " + e.Message + " servidor " + msws.Url);
                }
            }
            else
            {
                log.Debug("getCicloTimeoutEPadrao programa invalido para maquina " + maquina);
            }
            return retorno;
        }

        public bool setEventoInsert(idwws.eventoDTO evento)
        {
            try
            {
                LogEventInfo loggerInfo = new LogEventInfo(LogLevel.Trace, evento.maquina + "_trace_envio", "Enviando as: " + DateTime.Now + "|" + evento.origem);
                logInfo.Log(loggerInfo);
                getIdwWs().setEventoInsert(evento);
                loggerInfo = new LogEventInfo(LogLevel.Trace, evento.maquina + "_trace_envio", "ENVIADO as: " + DateTime.Now + "|" + evento.origem);
                logInfo.Log(loggerInfo);

                loggerInfo = new LogEventInfo(LogLevel.Trace, evento.maquina + "_trace_envio", "*************************************************");
                logInfo.Log(loggerInfo);

                return true;
            }
            catch (Exception e)
            {
                LogEventInfo loggerInfo = new LogEventInfo(LogLevel.Trace, evento.maquina + "_trace_envio", "Erro " + e.Message + " ao enviar as: " + DateTime.Now + "|" + evento.origem);
                logInfo.Log(loggerInfo);

                loggerInfo = new LogEventInfo(LogLevel.Trace, evento.maquina + "_trace_envio", "*************************************************");
                logInfo.Log(loggerInfo);

                return false;
            }
        }
    }
}
