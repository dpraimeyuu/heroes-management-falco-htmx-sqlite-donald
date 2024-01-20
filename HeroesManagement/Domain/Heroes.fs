module HeroesManagement.Domain

open System

type Species =
    | Human
    | Extraterrestrial
    static member toString spec =
        match spec with
        | Human -> "Human"
        | Extraterrestrial -> "Extraterrestrial"
    static member parse spec =
        match spec with
        | "Human" -> Human
        | "Extraterrestrial" -> Extraterrestrial
        | _ -> failwith "Fail to parse Species"

type Ability = Ability of string

type Hero =
    { Id: Guid
      Name: string
      Species: Species 
      Abilities: Ability array }

type Error =
    | GenericError of string
    | NotFoundError of string
    | DbError of (string * Exception)

type HeroesCollection<'T> =
    abstract member GetAll : unit -> Result<'T list, Error>
    abstract member Add    : 'T   -> Result<'T, Error>

let getHeroesUsing (storage : HeroesCollection<Hero>) =
    storage.GetAll

let createHeroUsing (storage : HeroesCollection<Hero>) hero =
    { hero with Id = Guid.NewGuid() }
    |> storage.Add