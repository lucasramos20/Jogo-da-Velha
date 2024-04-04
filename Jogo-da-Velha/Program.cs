using System;
using System.Collections.Generic;
using System.Linq;

namespace Jogo_da_Velha {
    class Program {
        static char[,] Tabu = new char[3, 3]; // Tabuleiro do jogo
        static bool TurnoHumano = true; // Variável para controlar de quem é a vez de jogar
        static char currentPlayer = 'X'; // Variável para controlar qual jogador está jogando

        static void Main(string[] args) {
            InicializaTabu();
            Console.WriteLine("Bem-vindo ao Jogo da Velha!");
            ImprimirTabu();

            while (!SeAcabouJogo()) {
                if (TurnoHumano) {
                    MovimentoHumano();
                } else {
                    MovimentoComputador();
                }

                ImprimirTabu();
                TurnoHumano = !TurnoHumano; // Alterna a vez de jogar entre o jogador e o computador
            }

            // Verifica o vencedor ou se o jogo terminou em empate
            char Vencedor = ChecarVitoria();
            if (Vencedor == 'X')
                Console.WriteLine("Parabéns! Você venceu!");
            else if (Vencedor == 'O')
                Console.WriteLine("O computador venceu!");
            else
                Console.WriteLine("Empate!");
        }

        // Inicializa o tabuleiro com espaços vazios
        static void InicializaTabu() {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    Tabu[i, j] = '-';
                }
            }
        }

        // Imprime o tabuleiro
        static void ImprimirTabu() {
            Console.WriteLine("  0 1 2");
            for (int i = 0; i < 3; i++) {
                Console.Write(i+3 + " ");
                for (int j = 0; j < 3; j++) {
                    Console.Write(Tabu[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        // Solicita e processa a jogada do jogador humano
        static void MovimentoHumano() {
            Console.WriteLine("Sua vez de jogar.");
            int row, col;
            int rowI;
            do {
                Console.WriteLine("Digite a linha (3-5): ");
                rowI = int.Parse(Console.ReadLine());
                if (rowI == 3) {
                    row = 0;
                } else if (rowI == 4 ) {
                    row = 1;
                } else {
                    row = 2;
                }
                Console.WriteLine("Digite a coluna (0-2): ");
                col = int.Parse(Console.ReadLine());
            } while (!SeMovimentoValido(row, col));
            Tabu[row, col] = 'X';
        }

        // Realiza a jogada do computador usando o algoritmo BFS
        static void MovimentoComputador() {
            Console.WriteLine("Vez do computador...");
            Console.WriteLine();
            int[] movimento = MovimentoBSF();
            Tabu[movimento[0], movimento[1]] = 'O';
        }

        // Verifica se a jogada é válida
        static bool SeMovimentoValido(int row, int col) {
            return row >= 0 && row < 3 && col >= 0 && col < 3 && Tabu[row, col] == '-';
        }

        // Verifica se o jogo terminou
        static bool SeAcabouJogo() {
            return ChecarVitoria() != '-' || IsBoardFull();
        }

        // Verifica se o tabuleiro está cheio
        static bool IsBoardFull() {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (Tabu[i, j] == '-')
                        return false;
                }
            }
            return true;
        }

        // Verifica o vencedor do jogo
        static char ChecarVitoria() {
            // Verifica linhas e colunas
            for (int i = 0; i < 3; i++) {
                if (Tabu[i, 0] == Tabu[i, 1] && Tabu[i, 1] == Tabu[i, 2] && Tabu[i, 0] != '-')
                    return Tabu[i, 0];
                if (Tabu[0, i] == Tabu[1, i] && Tabu[1, i] == Tabu[2, i] && Tabu[0, i] != '-')
                    return Tabu[0, i];
            }

            // Verifica diagonais
            if (Tabu[0, 0] == Tabu[1, 1] && Tabu[1, 1] == Tabu[2, 2] && Tabu[0, 0] != '-')
                return Tabu[0, 0];
            if (Tabu[0, 2] == Tabu[1, 1] && Tabu[1, 1] == Tabu[2, 0] && Tabu[0, 2] != '-')
                return Tabu[0, 2];

            return '-'; // Não há vencedor
        }

        // Obtém a melhor jogada para o computador usando o algoritmo BFS
        static int[] MovimentoBSF() {
            Queue<int[]> queue = new Queue<int[]>();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (Tabu[i, j] == '-') {
                        queue.Enqueue(new int[] { i, j });
                    }
                }
            }

            while (queue.Count > 0) {
                int[] move = queue.Dequeue();
                int row = move[0];
                int col = move[1];
                Tabu[row, col] = 'O';
                if (ChecarVitoria() == 'O') {
                    Tabu[row, col] = '-'; // Desfaz a jogada
                    return move;
                }
                Tabu[row, col] = '-'; // Desfaz a jogada
            }

            // Se não houver uma jogada vencedora imediata, retorna uma jogada aleatória
            Random rand = new Random();
            int randomRow, randomCol;
            do {
                randomRow = rand.Next(0, 3);
                randomCol = rand.Next(0, 3);
            } while (!SeMovimentoValido(randomRow, randomCol));
            return new int[] { randomRow, randomCol };
        }
    }
}




