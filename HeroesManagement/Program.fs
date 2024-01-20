module HeroesManagement.Program

open Falco
open Falco.Routing
open Falco.HostBuilder
open HeroesManagement.Domain
open HeroesManagement.Infrastructure
open HeroesManagement.UseCases

let handleError =
    function
    | GenericError message  -> Response.withStatusCode 400 >> Response.ofPlainText message
    | NotFoundError message -> Response.withStatusCode 404 >> Response.ofPlainText message
    | DbError (message, _)  -> Response.withStatusCode 500 >> Response.ofPlainText message

[<EntryPoint>]
let main args =
    let storage = HeroesSQLiteCollection()
    
    webHost args {
        endpoints [
            get "/" (StartingManagement.handleHome (getHeroesUsing storage))
            post "/app/new-hero" (AddingNewHero.handleCreateHero (createHeroUsing storage))
            get "/app/adding-new-hero" (Response.ofHtml AddingNewHero.Views.AddingNewHero)
            get "/app/browsing-hero" (BrowsingHeroes.handleBrowsingHeroes (getHeroesUsing storage) )
        ]
    }        
    0