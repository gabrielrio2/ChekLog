Log Processor
Este é um programa em C# que lê um arquivo de log, identifica blocos específicos de mensagens de erro e os processa para extrair informações úteis. O programa gera um arquivo de resultados contendo as informações extraídas.

Funcionalidade
Lê um arquivo de log especificado.
Extrai blocos de mensagens de log que começam com "Iniciando interpretação da mensagem..." e terminam com "Trabalho com pedidos foi finalizado.".
Procura por blocos que contêm o erro "System.ArgumentException: Parameter 'P_ID_TIPO_RESIDENCIA' not found in the collection.".
Extrai valores como RealmInicial e MensagemHash dos blocos com erro.
Escreve os resultados em um arquivo resultado.txt, onde cada RealmInicial é listado com os hashes de mensagem correspondentes.
Como Usar
1. Pré-requisitos
Certifique-se de ter o .NET SDK instalado no seu sistema. Se não tiver, baixe-o aqui.

2. Configuração do Caminho do Arquivo de Log
No código, há uma linha que define o caminho do arquivo de log:
csharp
Copiar código
string logFilePath = "C:/Users/bielr/Desktop/Teste/log.txt";
Altere essa linha para o caminho onde seu arquivo de log está localizado.

4. Executando o Programa
Compile e execute o programa com o comando:
bash
Copiar código
dotnet run
O programa irá:
Ler o arquivo de log.
Processar os blocos de mensagens de log.
Identificar blocos que contenham erros e extrair informações.
Gerar um arquivo resultado.txt na pasta onde o programa foi executado.

6. Saída
Após a execução, o arquivo resultado.txt será gerado. O conteúdo do arquivo será semelhante a:

arduino
Copiar código
RealmInicial_123
"MensagemHash_1",
"MensagemHash_2"

RealmInicial_456
"MensagemHash_3"
O terminal também exibirá mensagens informativas sobre o progresso, como o número de blocos encontrados e processados.

Estrutura do Código
Main: Função principal que orquestra a leitura e processamento do arquivo de log.
ExtractLogBlocks: Extrai blocos de log relevantes com base em padrões específicos de início e fim.
ProcessLogBlocks: Processa os blocos para verificar se há erros específicos e extrai as informações relevantes.
WriteResultsToFile: Escreve os resultados processados em um arquivo de texto.
PrintFileContent: Exibe o conteúdo do arquivo gerado.
Observações
Certifique-se de que o caminho do arquivo de log está correto e que o programa tem permissão para acessá-lo.
O arquivo resultado.txt será salvo no diretório onde o programa é executado (/bin/Debug/net8.0/ ou equivalente).
Licença
Este projeto é de código aberto e pode ser modificado conforme necessário.
