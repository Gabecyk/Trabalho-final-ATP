using System;
using System.IO;
using System.Text;

class Program 
{
    public static void Main(String[] args) 
    {
        bool continuar = true;//bool para parar a repetição ao escolher opc 6
        while (continuar == true) //while até escolher 6
        {

            //opções
            Console.Clear();
            Console.WriteLine("Escolha uma das opções:");
            Console.WriteLine("1 - Atualizar estoque");
            Console.WriteLine("2 - Registrar venda");
            Console.WriteLine("3 - Relatório de vendas");
            Console.WriteLine("4 - Relatório de estoque");
            Console.WriteLine("5 - Criar arquivo de vendas");
            Console.WriteLine("6 - Sair");
            int n = int.Parse(Console.ReadLine()); //lê a escolha

            switch (n) //switch que lava ao procedimento, conforme a opção escolhida
            {
                case 1:                    
                    atualizarEstoque();
                    break;
                case 2:
                    registrarVenda();
                    break;
                case 3:
                    relatorioVendas();
                    break;
                case 4:
                    relatorioEstoques();
                    break;
                case 5:
                    string filePath = "D://vendas.txt"; //caso ja tenha um arquivo preparado, ele irá apontar e perguntar se quer ou não fazer outro, caso não tenha irá fazer um
                    if(File.Exists(filePath)){
                    Console.WriteLine("Ja existe o arquivo, deseja refaze-lo mesmo?");
                    Console.WriteLine("'s' para sim e 'n' para não.");
                    char res = char.Parse(Console.ReadLine().ToLower());
                    if(res == 's'){
                        criarVendas();
                    }
                    }else{
                    Console.WriteLine("Alteração não feita!");
                    Console.ReadKey();
                    }
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("Programa encerrado!");
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("Escolha de 1 - 6!");
                    break;
            }
        }
    }

    static void atualizarEstoque()//atualiza o estoque e salva no arquivo
    {
        Console.Clear();
        string filePath = "D://vendas.txt";

        if(File.Exists(filePath))//caso não tenha o arquivo ele irá pedir para fazer a opc 5 primeiro
        {
            StreamReader sr = new StreamReader("D://vendas.txt");//abre arquivo para ler e receber
            string produtoLine = sr.ReadLine();
            string estoqueLine = sr.ReadLine();
            string mtzLine = sr.ReadLine();
            sr.Close(); //fecha arquivo

            //Passa elas para vetores, usando split para separar
            string[] produtos = produtoLine.Split(',');
            int[] estoque = Array.ConvertAll(estoqueLine.Split(','), int.Parse);
            int[] mtz = Array.ConvertAll(mtzLine.Split(','), int.Parse);

            Console.WriteLine("Produtos e seus estoques:"); 
            for (int i = 0; i < produtos.Length; i++){ //Mostra os produtos que tem e o estoque
                Console.WriteLine($"Produto: {produtos[i]}, estoque: {estoque[i]}.");
            }
            
                Console.WriteLine("Qual produto quer aumentar o estoque?");
                Console.WriteLine("Digite o número referente ao produto:");
                int n = 0;
                for (int i = 0; i < produtos.Length; i++){//escolher qual produto vai mudar o estoque
                    Console.WriteLine($"{i+1} - {produtos[i]}.");
                }
                n = int.Parse(Console.ReadLine()); //recebe o numero do produto
                n = n-1;//ele menos 1 para entrar no vetor e pegar ele
                Console.WriteLine($"Você selecionou o produto: {produtos[n]} com o estoque de {estoque[n]}.");
                Console.WriteLine("Digite o valor novo para o estoque:");
                estoque[n] = int.Parse(Console.ReadLine());//novo valor de estoque para o produto

                StreamWriter sw = new StreamWriter(filePath, false, Encoding.ASCII); //salva tudo no arquivo
                sw.WriteLine(string.Join(",", produtos));
                sw.WriteLine(string.Join(",", estoque));
                sw.WriteLine(string.Join(",", mtz));
                sw.Close();//fecha

                Console.WriteLine("Estoque atualizado com sucesso!");
                Console.WriteLine("Aperte enter para continuar.");
                Console.ReadKey();

        }
        else {
            Console.WriteLine("Crie um arquivo primeiro, na opção 5.");
            Console.WriteLine("Pressione enter, para continuar");
            Console.ReadKey();
        }
    }

    static void registrarVenda() //registra a venda
    {
        
        Console.Clear();


        if(File.Exists("D://vendas.txt")){ //caso não tenha o arquivo ele irá pedir para fazer a opc 5 primeiro
        StreamReader sr = new StreamReader("D://vendas.txt");
        string produtoLine = sr.ReadLine();
        string estoqueLine = sr.ReadLine();
        string mtzLine = sr.ReadLine();
        
        sr.Close();
        

        string[] produtos = produtoLine.Split(',');
        int[] estoque = Array.ConvertAll(estoqueLine.Split(','), int.Parse);
        int[] mtz = Array.ConvertAll(mtzLine.Split(','), int.Parse);

        Console.WriteLine($"Qual produto foi vendindo?");
        int n = int.MaxValue;//usando max para não ocorrer erro no while
        int nn = produtos.Length;
        while(n > nn){//while até digitar o numero do produto
            Console.WriteLine("Digite o número referente ao produto vendido.");
            for (int i = 0; i < produtos.Length; i++) { //for para escrever todas as informações de produtos e do estoque
                Console.WriteLine($"{i+1} - {produtos[i]}.");
            }
            int num = int.Parse(Console.ReadLine());
            if(num > produtos.Length){
                Console.Clear();
                Console.WriteLine("Digite um número dentro do que foi pedido");
                Console.WriteLine("Pressione enter para continuar.");
                Console.ReadKey();
            }
            else{
            n = num;
            n = n-1;

            }
        }

        if (estoque[n] < 1) { //ve se não ja está como zero o estoque do produto escolhido
                Console.WriteLine("O estoque desse produto ja está à zero");
                Console.WriteLine("Atualize no 'Atualiza estoque' primeiro, e tente novamente!");
                Console.WriteLine("Pressione enter, para continuar.");
                Console.ReadKey();
                
            }
            else{
                Console.WriteLine("Qual a quantidade vendida desse produto?"); //qtd vendida
                int nu = int.Parse(Console.ReadLine());

                if (nu > estoque[n]) { //ve se não é maior do que a no estoque
                    Console.WriteLine("Essa quantidade passa do estoque armazenado.");
                    Console.WriteLine("Atualize o estoque, ou tente novamente com outra quantidade!"); 
                    Console.WriteLine("Pressione enter, para continuar.");
                    Console.ReadKey();
                }
                else if(nu <= estoque[n]){ //dimuniu a qtd
                    estoque[n] = estoque[n] - nu;  

                    Console.WriteLine("Digite o dia da venda (1-31):"); //dia da venda
                    int dia = int.Parse(Console.ReadLine()) - 1;

                    mtz[dia] += nu;  // Registrar venda na matriz  

                    StreamWriter sw = new StreamWriter("D://vendas.txt", false, Encoding.ASCII);//atualiza arquivo
                    sw.WriteLine(string.Join(",", produtos));
                    sw.WriteLine(string.Join(",", estoque));
                    sw.WriteLine(string.Join(",", mtz));
                    sw.Close();  

                    Console.WriteLine("Atualizado com sucesso!");
                    Console.WriteLine("Pressione enter para continuar");
                    Console.ReadKey();           
                }
            }

        }
        else{
            Console.WriteLine("Crie um arquivo primeiro, na opção 5.");
            Console.WriteLine("Pressione enter, para continuar");
            Console.ReadKey();
        }
        
    }

    static void relatorioVendas() //relatorio das vendas
    {
        Console.Clear();
        string filePath = "D://vendas.txt"; //caso não tenha o arquivo ele irá pedir para fazer a opc 5 primeiro

        if(File.Exists(filePath)){
        StreamReader sr = new StreamReader("D://vendas.txt");
        string produtoLine = sr.ReadLine();
        string estoqueLine = sr.ReadLine();
        string mtzLine = sr.ReadLine();
        
        sr.Close();
        

        string[] produtos = produtoLine.Split(',');
        int[] estoque = Array.ConvertAll(estoqueLine.Split(','), int.Parse);
        int[] mtz = Array.ConvertAll(mtzLine.Split(','), int.Parse);
        Console.WriteLine("Resultado de vendas do mês:");
        for(int i = 0; i < mtz.Length; i++) //mostra as vendas de cada dia do mês
        {
            Console.Write($"Dia {i + 1} - {mtz[i]} \t");
            if(i == 6 || i == 13 || i == 20 || i == 27)
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine("Aperte enter, para continuar.");
        Console.ReadKey();

        }
        else{
            Console.WriteLine("Crie um arquivo primeiro, na opção 5.");
            Console.WriteLine("Pressione enter, para continuar");
            Console.ReadKey();
        }
    }

    static void relatorioEstoques() //Mostra o estoque atualizado
    {
         Console.Clear();
        string filePath = "D://vendas.txt"; 

        if(File.Exists(filePath)) //caso não tenha o arquivo ele irá pedir para fazer a opc 5 primeiro
        {
            if(File.Exists(filePath)){
                StreamReader sr = new StreamReader("D://vendas.txt");
                string produtoLine = sr.ReadLine();
                string estoqueLine = sr.ReadLine();
                sr.Close();
        

                string[] produtos = produtoLine.Split(',');
                int[] estoque = Array.ConvertAll(estoqueLine.Split(','), int.Parse);

                Console.WriteLine("Relatório do estoque de produtos:");
                for(int i = 0; i < produtos.Length; i++){ //for para mostrar os produtos e seus respectivos estoques
                Console.WriteLine($"Produto: {produtos[i]}, Estoque: {estoque[i]}.");
                }
                Console.WriteLine("Pressione enter, para continuar.");
                Console.ReadKey();
            }

        }
        else{
            Console.WriteLine("Crie um arquivo primeiro, na opção 5.");
            Console.WriteLine("Pressione enter, para continuar");
            Console.ReadKey();
        }
    }

    static void criarVendas() //cria o arquivo
    {
        StreamWriter sw = new StreamWriter("D://vendas.txt", false, Encoding.ASCII);
        Console.WriteLine("Quantos produtos serão colocados: "); //quantos produtos serão colocados
        int n = int.Parse(Console.ReadLine());

        string[] produtos = new string[n];
        for (int i = 0; i < n; i++)
        {
            Console.Write($"Nome do produto {i + 1}: ");//nome do produto de cada um
            produtos[i] = Console.ReadLine();
        }
        sw.WriteLine(string.Join(",", produtos));//escreve no arquivo, separado por ","

        int[] estoque = new int[n];
        Console.WriteLine("Agora digite o estoque de cada produto.");//estoque de cada produto
        for (int i = 0; i < n; i++)
        {
            Console.Write($"Digite o estoque do produto {produtos[i]}: ");
            estoque[i] = int.Parse(Console.ReadLine());
        }
        sw.WriteLine(string.Join(",", estoque)); //escreve no arquivo, separado por ","

        int[] mtz = new int[31]; //mtz para guardar as vendas do mês, começa zerado
        for (int i = 0; i < 31; i++){
            mtz[i] = (0);
        }
        sw.WriteLine(string.Join(",", mtz));//salva no arquivo

        sw.Close();    //fecha arquivo

        Console.WriteLine("Arquivo criado!");
        Console.WriteLine("Pressione enter, para continuar.");
        Console.ReadKey();   
    }
}
