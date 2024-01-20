namespace HeroesManagement.UseCases


module StartingManagement =
    open Falco.Htmx
    open HeroesManagement.Domain
    open Falco
    open Falco.Markup
    
    let handleHome (getHeroesUseCase: (unit -> Result<Hero list,Error>))=                 
        fun ctx ->
            let html =
                Templates.html5 "en" [
                    Elem.link [
                        Attr.href "style.css"
                        Attr.rel "stylesheet"
                    ]
                    Elem.script [ Attr.src Script.src ] []
                ] [
                    Elem.body [] [
                        Elem.h1 [] [ Text.raw "Hello from Falco.Htmx!" ]
                        BrowsingHeroes.Views.BrowsingHeroesView getHeroesUseCase
                    ]
                ]

            Response.ofHtml html ctx

