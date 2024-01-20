namespace HeroesManagement.UseCases

[<RequireQualifiedAccess>]
module BrowsingHeroes =
    open Falco
    open System
    open Falco.Markup
    open HeroesManagement.Contracts
    
    open Falco.Htmx
    open HeroesManagement.Domain
    
    let private toHeroesList =
        List.map HeroesMapping.toDto >>
        List.map (fun hero ->
                            Elem.div [Attr.style "margin-bottom: 10px;"] [
                                Elem.div [] [
                                    Elem.span [Attr.style "margin-right: 10px;"] [Text.raw "Name:"]
                                    Elem.span [] [Text.raw hero.Name]
                                ]
                                Elem.div [] [
                                    Elem.span [Attr.style "margin-right: 10px;"] [Text.raw "Species:"]
                                    Elem.span [] [Text.raw hero.Species]
                                ]
                                Elem.div [] [
                                    Elem.span [Attr.style "margin-right: 10px;"] [Text.raw "Abilities:"]
                                    Elem.span [] [Text.raw (String.Join(", ", hero.Abilities))]
                                ]
                            ]             
                        )
    let private ExistingHeroesList (getHeroesUseCase: (unit -> Result<Hero list,Error>)) =
        let heroes =
            getHeroesUseCase ()
            |> function
                | Ok heroes when heroes |> List.isEmpty -> None
                | Ok heroes ->
                    let heroesEl = heroes |> toHeroesList
                    Some (Elem.div [] heroesEl, heroes |> List.length)
                | Error error -> None
        match heroes with
        | None -> Elem.div [] []
        | Some (heroes, heroesCount) ->
            Elem.div [] [
            Elem.h4 [] [Text.rawf $"Existing heroes ({heroesCount})"]
            Elem.div [] [heroes]
        ]
    let private MainViewActions =
        Elem.div [] [
            Elem.h4 [] [ Text.raw "Actions:" ]
            Elem.div [] [
                Elem.button [
                    Hx.get "/app/adding-new-hero"
                    Hx.target (Hx.Target.css "#content")
                    Hx.swap Hx.Swap.innerHTML
                    Hx.trigger [(Hx.Trigger.event("click", None, List.empty<_>))]
                ] [Text.raw "Add new hero"]
            ]
        ]
    
    module Views =
        let BrowsingHeroesView (getHeroesUseCase: (unit -> Result<Hero list,Error>)) =
            Elem.div [Attr.id "content"] [
                        MainViewActions
                        ExistingHeroesList getHeroesUseCase
                    ]

    let handleBrowsingHeroes getHeroesWithStorage = fun ctx ->
        ctx
        |> Response.ofHtml (Views.BrowsingHeroesView getHeroesWithStorage)