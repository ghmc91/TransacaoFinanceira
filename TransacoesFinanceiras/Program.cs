//Obs: Voce é livre para implementar na linguagem de sua preferência, desde que respeite as funcionalidades e saídas existentes, além de aplicar os conceitos solicitados.

namespace TransacaoFinanceira
{
    class Program
    {

        static void Main(string[] args)
        {
            var TRANSACOES = new[] {new Transacoes(1, "09/09/2023 14:15:00", 938485762, 214748364, 150),
                                     new Transacoes(2, "09/09/2023 14:15:05", 214748364, 210385733, 149),
                                     new Transacoes(3, "09/09/2023 14:15:29", 347586970, 238596054, 1100),
                                     new Transacoes(4, "09/09/2023 14:17:00", 675869708, 210385733, 5300),
                                     new Transacoes(5, "09/09/2023 14:18:00", 238596054, 674038564, 1489),
                                     new Transacoes(6, "09/09/2023 14:18:20", 573659065, 563856300, 49),
                                     new Transacoes(7, "09/09/2023 14:19:00", 938485762, 214748364, 44),
                                     new Transacoes(8, "09/09/2023 14:19:01", 573659065, 675869708, 150),

            };
            Transacoes executor = new Transacoes();

            /**
             * Utilizando essa forma paralelizada de iteração, as transações são processadas concorrentemente.
             * Entendo que deva ser respeitada uma ordem no processamento das transações, por isso utilizo um laço de repetição comum
             * Além disso, por se tratarem de operações bem simples dentro do laço, acredito que não haja um ganho computacional
             * abrindo novas threads
             **/

            //Parallel.ForEach(TRANSACOES, item =>
            //{
            //    executor.transferir(item.CorrelationId, item.OriginAccount, item.DestinyAccount, item.Value);
            //}) ;
            TRANSACOES.ToList().ForEach(item =>
            {
                executor.transferir(item.CorrelationId, item.OriginAccount, item.DestinyAccount, item.Value);
            });

        }
    }


    public class Transacoes : acessoDados
    {
        public int CorrelationId { get; set; }

        public DateTime TransactionDate { get; set; }

        public int OriginAccount { get; set; }

        public int DestinyAccount { get; set; }

        public decimal Value { get; set; }

        public Transacoes()
        {

        }

        public Transacoes(int correlationId, string transactionDate, int originAccount, int destinyAccount, decimal value)
        {
            CorrelationId = correlationId;
            TransactionDate = DateTime.Parse(transactionDate);
            OriginAccount = originAccount;
            DestinyAccount = destinyAccount;
            Value = value;
        }
        public void transferir(int correlationId, int originAccount, int destinyAccount, decimal value)
        {
            contas_saldo conta_saldo_origem = getSaldo<contas_saldo>(originAccount);
            if (conta_saldo_origem.saldo < value)
            {
                Console.WriteLine("Transacao numero {0 } foi cancelada por falta de saldo", correlationId);

            }
            else
            {
                contas_saldo conta_saldo_destino = getSaldo<contas_saldo>(destinyAccount);
                conta_saldo_origem.saldo -= value;
                conta_saldo_destino.saldo += value;
                Console.WriteLine("Transacao numero {0} foi efetivada com sucesso! Novos saldos: Conta Origem:{1} | Conta Destino: {2}", correlationId, conta_saldo_origem.saldo, conta_saldo_destino.saldo);
            }
        }
    }
    public class contas_saldo
    {
        public contas_saldo(int conta, decimal valor)
        {
            this.conta = conta;
            this.saldo = valor;
        }
        public int conta { get; set; }
        public decimal saldo { get; set; }
    }

    public class acessoDados
    {
        Dictionary<int, decimal> SALDOS { get; set; }
        private List<contas_saldo> TABELA_SALDOS;
        public acessoDados()
        {
            TABELA_SALDOS = new List<contas_saldo>();
            TABELA_SALDOS.Add(new contas_saldo(938485762, 180));
            TABELA_SALDOS.Add(new contas_saldo(347586970, 1200));
            TABELA_SALDOS.Add(new contas_saldo(214748364, 0));
            TABELA_SALDOS.Add(new contas_saldo(675869708, 4900));
            TABELA_SALDOS.Add(new contas_saldo(238596054, 478));
            TABELA_SALDOS.Add(new contas_saldo(573659065, 787));
            TABELA_SALDOS.Add(new contas_saldo(210385733, 10));
            TABELA_SALDOS.Add(new contas_saldo(674038564, 400));
            TABELA_SALDOS.Add(new contas_saldo(563856300, 1200));


            SALDOS = new Dictionary<int, decimal>();
            this.SALDOS.Add(938485762, 180);

        }
        public T getSaldo<T>(int id)
        {
            return (T)Convert.ChangeType(TABELA_SALDOS.Find(x => x.conta == id), typeof(T));
        }
        public bool atualizar<T>(T dado)
        {
            try
            {
                contas_saldo item = (dado as contas_saldo);
                TABELA_SALDOS.RemoveAll(x => x.conta == item.conta);
                TABELA_SALDOS.Add(dado as contas_saldo);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

    }
}
