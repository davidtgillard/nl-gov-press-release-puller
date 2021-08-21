open iText.Html2pdf
open System.IO
open HtmlAgilityPack
open System.Text.RegularExpressions
open System


[<EntryPoint>]
let main argv =

  /// Finds all links within a given docpath that match a given regular expression. Does not perform I/O.
  let findLinks (docUri: Uri) (doc: HtmlDocument) (rexp: Regex) =
    doc.DocumentNode.Descendants("a") 
    |> Seq.filter (fun node -> node.Attributes.Contains "href") // filter out all 'a' nodes that don't have a 'href' attribute
    |> Seq.map (fun node -> node.Attributes.["href"].Value) // get the link strings
    |> Seq.filter (fun link -> (rexp.Match link).Success) // match against the filter passed in
    |> Seq.map (fun link -> // build the URI of the link
                  let uri = try  
                              Uri link
                            with | _ -> Uri(docUri, link) // if the link is relative, make it absolute           
                  let builder = UriBuilder uri
                  builder.Scheme <- docUri.Scheme // assume same scheme as docUri
                  builder.Port <- -1 // no port
                  builder.Uri)
    |> Seq.toList
  
  /// Finds all links for which there is a link-path from rootUri that matches the list of patterns given, such that the first link in the path matches pattern[0], the second link in the path matches pattern[1], etc.
  let findAllLinks (rootUri: Uri) (patterns: Regex array) =
    // visits all the link/path-depth pairs given by toVisit. Any matching links that match patterns[end] at the end of the path are returned. 
    let rec findem (toVisit: (Uri * int) list) (finalLinks: Uri list) =
      if List.isEmpty toVisit then // if 
        finalLinks
      else
        let currentUri, level = toVisit.Head
        let links = findLinks currentUri (HtmlWeb().Load(currentUri)) patterns.[level]
        if level < patterns.Length-1 then
          findem (toVisit.Tail @ (links |> List.map (fun l -> l, level+1))) finalLinks
        else 
          findem toVisit.Tail (finalLinks @ links)
    // kick off the recursion
    findem [(rootUri, 0)] List.empty

  let usage = sprintf "Usage: %s URL OUTPUT-FOLDER LINK-REGEX-PATTERN1 ?LINK-REGEX-PATTERN2?..." (System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)
  
  if Array.length argv < 3 then
    eprintfn "Error: Not enough arguments provided."
    eprintfn "%s" usage

  /// The CLI args.
  let rootUrl = Uri argv.[0]
  let outputFolder = argv.[1]
  let patterns = argv.[2..] |> Array.map Regex

  Directory.CreateDirectory outputFolder |> ignore

  // for all links, try to download them and convert them to pdf
  for lp in findAllLinks rootUrl patterns do
    printfn "attempting to load %s" (lp.ToString())
    let html = HtmlWeb().Load(lp).Text
    let lastSegment = (Array.last lp.Segments)
    let last = if lastSegment.LastIndexOf('/') = lastSegment.Length-1 then lastSegment.[0..lastSegment.Length-2] else lastSegment

    let outputFile = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension last + ".pdf")
    printfn "%s --> %s" (lp.ToString()) outputFile
    // now we'll try to convert
    use fStream = new FileStream(outputFile, FileMode.Create)
    try
      HtmlConverter.ConvertToPdf(html, fStream)
    with | ex -> printfn "Error converting file %s to PDF: %s" (lp.ToString()) (ex.ToString())
    fStream.Close()
  
  
  0