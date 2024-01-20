namespace HeroesManagement.Contracts

open HeroesManagement.Domain

[<CLIMutable>]
type HeroInput =
    { Name: string
      Species: string
      Abilities: string array }

[<RequireQualifiedAccess>]
module HeroesMapping =
    let toDto hero =
        {| Id = hero.Id.ToString()
           Name = hero.Name
           Species = hero.Species |> Species.toString 
           Abilities = hero.Abilities |> Array.map (fun (Ability ab) -> ab) |}
    
    let fromDto id input =
        { Id = id
          Name = input.Name
          Species = (input.Species |> Species.parse)
          Abilities = (input.Abilities |> Array.map (fun ab -> Ability ab)) }