namespace Main;
using Extends;

public class School {
    
    public void FinalAssessment() {
        while(true) {
            Console.WriteLine("Digite as notas do aluno separadas por virgula(ex: 5.5,8,10):");
            try {
                var parameters = Console.ReadLine();
                if(string.IsNullOrEmpty(parameters))
                    throw new Exception("entrada inválida, tente novamente");
                
                var testNotes = this.getTestNotes(parameters);
                
                var average = testNotes.ArithmeticMean();
                
                if(average <= 4.9) 
                    Console.WriteLine($"Aluno em recuperação com média {average}!");
                else if (average <= 6.9)
                    Console.WriteLine($"Aluno em prova final com média {average}!");
                else 
                    Console.WriteLine($"Aluno aprovado com a média {average}!");

                if(average <= 6.9) {
                    var requiredNote = (7 - average) + 7;
                    if(requiredNote <= 10)
                        Console.WriteLine($"O aluno precisa de nota {requiredNote} para ser aprovado!");
                    else 
                        Console.WriteLine($"O aluno está reprovado, pois precisaria de uma nota {requiredNote} para atingir a média necessária para aprovação.");
                }

                var response = "";
                var expecteds = new string[]{ "s", "n" };
                while(!expecteds.Contains(response)) {
                    Console.WriteLine("Deseja verificar mais alunos(s/n)?");
                    response = Console.ReadLine()?.Trim()?.ToLower();
                    if(!expecteds.Contains(response)) {
                        Console.WriteLine("Resposta inválida, responsa com 's' para Sim ou 'n' para Não.");
                        continue;
                    }
                    if(response == "n") {
                        Console.WriteLine("Programa finalizado!");
                        return;
                    }  else 
                        break;
                }
            } 
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                continue;
            }
        }
    }

    private float[] getTestNotes(string testNotes) {
        float[] resultNotes = [];
        var stringNotes = testNotes.Replace(" ", "").Split(",");
        if(stringNotes.Length != 3)
            throw new Exception("Entrada inválida, são esperadas 3 notas!");

        return stringNotes.ToFloatArray((value) => {
            if(value > 10) throw new Exception("Entrada inválida, notas devem ser números de 0 a 10!");
            return value;
        });
    }

}
