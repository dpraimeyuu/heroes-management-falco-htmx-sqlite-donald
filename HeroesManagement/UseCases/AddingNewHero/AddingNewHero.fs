namespace HeroesManagement.UseCases

module AddingNewHero =

    open Falco
    open System
    open Falco.Markup
    open HeroesManagement.Contracts

    let handleCreateHero createHeroUseCase : HttpHandler =
        (fun ctx -> task {
                 let! query = Request.getForm ctx
                     
                 let heroInput: HeroInput = {
                     Name = query.GetString("name")
                     Species =  query.GetString("species")
                     Abilities = query.GetString("abilities").Split(',') 
                 }
                 
                 return HeroesMapping.fromDto Guid.Empty heroInput
                 |> createHeroUseCase
                 |> function
                     | Ok hero     ->
                         let dto = hero |> HeroesMapping.toDto
                         Response.ofHtml (
                             Elem.div [] [
                                 Text.rawf ($"Hero created! Id: {dto.Id}")
                             ]
                         )
                     | Error error ->
                         Response.ofHtml (
                             Elem.div [] [
                                 Text.rawf ($"Sorry, something bad happened: {error.ToString()}")
                             ]
                        )
                <| ctx
            }
        )

    [<RequireQualifiedAccess>]
    module Views =
        open Falco.Htmx
        let AddingNewHero =
            (Elem.div [] [
                 Elem.div [] [
                            Elem.h4 [] [ Text.raw "Actions:" ]
                            Elem.div [] [
                                Elem.h3 [] [Text.raw "Add new hero"]
                                Elem.form [Attr.id "adding-new-hero"; Hx.post "/app/new-hero"; Attr.style "width: 20%;display: flex; flex-direction: column;"] [
                                    Elem.div [Attr.style "display: flex; flex-direction: column; margin-left: 10px; margin-right: 10px; justify-content: space-between; align-items: center"] [
                                        Elem.div [Attr.style "margin-bottom: 10px;"] [
                                            Elem.label [Attr.for' "name"] []
                                            Elem.input [Attr.name "name"]
                                        ]
                                    
                                        Elem.div [Attr.style "margin-top: 10px; margin-bottom: 10px;"] [
                                            Elem.label [Attr.for' "species"] []
                                            Elem.input [Attr.name "species"]
                                        ]
                                        
                                        Elem.div [Attr.style "margin-top: 10px; margin-bottom: 10px;"] [
                                            Elem.label [Attr.for' "abilities"] []
                                            Elem.input [Attr.name "abilities"; Attr.placeholder "Comma separated abilities"]
                                        ]
                                    ]
                                    
                                    Elem.div [Attr.style "justify-content: center;display: flex;margin-right: auto;margin-left: auto;padding-left: 8rem;"] [
                                        Elem.button [] [ Text.raw "Submit" ]
                                    ]
                                ]
                                Elem.button [Attr.style "margin-top: 10px;"; Hx.get "/app/browsing-hero"; Hx.target (Hx.Target.css "#content"); Hx.swap Hx.Swap.innerHTML  ] [Text.raw "Go back"]
                            ]
                        ]
            ])