using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using wapi.Models;

namespace wapi.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Ohtml { get; set; }
        public string Ahtml { get; set; }
        public string Shortdesc { get; set; }
        public string TopicCategory { get; set; }
        // public Perskill Perskill { get; set; }
        public string Hazards { get; set; }

        public string Skill { get; set; }
        public string Trade { get; set; }
        public string Duration { get; set; }
        public string Num { get; set; }

        public Topic CreateTopic(string tpath, string fname)
        {
            // convert path
            tpath = tpath.Replace("::", "/");

            HtmlDocument x = new HtmlDocument();
            Console.WriteLine("/data/" + tpath + "/" + fname);
            x.Load(Params.ServerPath + Params.UserDoc + tpath + "/" + fname);
            HtmlNode htmlbody = x.DocumentNode.SelectSingleNode("//body");

            Console.WriteLine(htmlbody.ToString());

            // replace imagelinks
            var images = htmlbody.SelectNodes("//img");
           // var imgjson = "";

            if (images != null)
            {
                for (int i = 0; i < images.Count; i++)
                {
                    var src = images[i].Attributes["src"].Value;
                    if (src.IndexOf("images") > -1) { src = "images" + src.Split("images")[1]; }
                    else if (src.IndexOf("image") > -1) { src = "image" + src.Split("image")[1]; }
                    else if (src.IndexOf("Images") > -1) { src = "Images" + src.Split("Images")[1]; }
                    images[i].Attributes["src"].Value = Params.UserDoc + tpath.Split("/")[0] + "/" + src;
                }
            }

            // transform figure divs into angular cards
            // convert figures to divs
            var figelems = htmlbody.SelectNodes("//figure");

            var figure = htmlbody.SelectNodes("//div[@class='fig fignone']");

            if (figure == null) {  figure = htmlbody.SelectNodes("//figure"); }
            var fig1 = figure;
            if (fig1 != null)
            {
                for (int i=0;i<figure.Count;i++)
                {
                    //captiontext
                    var caption = figure[i].SelectSingleNode("span[@class='figcap']");
                    
                    var captiontxt = "";
                    if (caption != null)
                    {
                        Console.WriteLine("HTMLCAPTION: " + caption.InnerText);
                        var figtitle = caption.SelectSingleNode("span");
                        caption.RemoveChild(figtitle);
                        captiontxt = caption.InnerText;
                    }

                    // image
                    var img = figure[i].SelectSingleNode("img");
                    var imgsrc = "";
                    if (img != null)
                    {
                        imgsrc = img.Attributes["src"].Value;
                    }

                    // legend
                    var legend = figure[i].SelectSingleNode("dl");
                    var legitems = "";
                    if (legend != null)
                    {
                        var dds = legend.SelectNodes("dd");
                        for (int j = 0; j<dds.Count;j++)
                        {
                            legitems = legitems + "<li>" + dds[j].InnerText + "</li>";
                        }
                       
                    }
                    var figcode =   @"<div class='fig-webapi'>"+
                                    "<div class='figcaption-webapi'>"+ captiontxt + "</div>"+
                                    "<div class='figimg-webapi'><img src='"+ imgsrc + "' alt='"+ captiontxt + "'></div>"+
                                    "<div class='figlegend-webapi'>"+
                                    "<ol>"+legitems+"</ol></div></div";
                    // create Angular element
                    HtmlNode AngularFigure = HtmlNode.CreateNode(figcode);
                    Console.WriteLine("FIGURETYPE: " + figcode);
                    figure[i].ParentNode.ReplaceChild(AngularFigure, figure[i]);
                    
                }
            }



            // convert links to angular routes


            // extract hazard statements
            var hazstatement = x.DocumentNode.SelectNodes("//table[@class='note hazardstatement']");
            
            if (hazstatement != null)
            {
                for (int i = 0; i < hazstatement.Count; i++)
                {
                    var hazimg = hazstatement[i].SelectSingleNode("//td[1]/img").GetAttributeValue("src", "warning.svg");
                    hazimg = hazimg.Substring(hazimg.LastIndexOf("/") + 1);

                    var hazpath = tpath.Split("/")[0];
                    var hazpanel = hazstatement[i].SelectSingleNode("//td[2]").InnerHtml;
                    var hazard = @"<div class='haz'><div class='hazimg'><img src='" + Params.UserDoc + hazpath + "/symbols/" + hazimg + "' /></div>" + hazpanel + "</div>";

                    HtmlNode AngularHazard = HtmlNode.CreateNode(hazard);
                    hazstatement[i].ParentNode.ReplaceChild(AngularHazard, hazstatement[i]);
                }

            }

            // set topic category
            //ar tcat = x.DocumentNode.SelectSingleNode("//meta[@name='DC.type']").Attributes["content"].Value;
            var category = x.DocumentNode.SelectNodes("//meta[@name='DC.type']");
            var tcat = "";
            if (category != null)
            {
                for (int i = 0; i < category.Count; i++)
                {
                    tcat = category[i].Attributes["content"].Value;
                    
                }
            }

            // set perskill
            //Perskill psk = new Perskill();
            /*psk.Skill = x.DocumentNode.SelectSingleNode("//li[@class='li li perskill']").InnerText;
            psk.Duration = x.DocumentNode.SelectSingleNode("//li[@class='li li esttime']").InnerText;
            psk.Trade = x.DocumentNode.SelectSingleNode("//li[@class='li li perscat']").InnerText;
            psk.Num = x.DocumentNode.SelectSingleNode("//li[@class='li li personnel']").InnerText;
            */
            


            //var tcat = "task";
            // create Ahtml

            Topic newTopic = new Topic();

            if (x.DocumentNode.SelectSingleNode("//p[@class='shortdesc']") != null)
            {
                newTopic.Shortdesc = x.DocumentNode.SelectSingleNode("//p[@class='shortdesc']").InnerText;
            }
            else
            {
                newTopic.Shortdesc = "Short description";
            }
            newTopic.Ohtml = x.DocumentNode.SelectSingleNode("//html").InnerHtml;
            newTopic.TopicCategory = tcat;
            newTopic.Path = tpath;
            newTopic.FileName = fname;
            //newTopic.Perskill = psk;
            if (x.DocumentNode.SelectSingleNode("//li[@class='li li perskill']") != null) newTopic.Skill = x.DocumentNode.SelectSingleNode("//li[@class='li li perskill']").InnerText;
            if (x.DocumentNode.SelectSingleNode("//li[@class='li li esttime']") != null) newTopic.Duration = x.DocumentNode.SelectSingleNode("//li[@class='li li esttime']").InnerText;
            if (x.DocumentNode.SelectSingleNode("//li[@class='li li perscat']") != null) newTopic.Trade = x.DocumentNode.SelectSingleNode("//li[@class='li li perscat']").InnerText;
            if (x.DocumentNode.SelectSingleNode("//li[@class='li li personnel']") != null) newTopic.Num = x.DocumentNode.SelectSingleNode("//li[@class='li li personnel']").InnerText;
            /*
            var nodetoremovepsk = x.DocumentNode.SelectSingleNode("//ul[@class='ul ul reqpers']");
            if (nodetoremovepsk != null)
            {
                nodetoremovepsk.Remove();
                Console.WriteLine("has node been removed psk? " + nodetoremovepsk.InnerText);
            }
            */

            newTopic.Ahtml = htmlbody.InnerHtml;
            return newTopic;
        }
        
    }

    public class Perskill
    {
        public int Id { get; set; }
        public string Skill { get; set; }
        public string Trade { get; set; }
        public string Duration { get; set; }
        public string Num { get; set; }
    }
    public class Consumable
    {
        public int Id { get; set; }
        public string Consumablename { get; set; }
        public string Consumableref { get; set; }

    }
    public class ConsumablesList
    {
        public int Id { get; set; }
        public List<Consumable> consumables = new List<Consumable>();
    }

    public class Spare
    {
        public int Id { get; set; }
        public string Sparename { get; set; }
        public string Spareref { get; set; }

    }
    public class SparesList
    {
        public int Id { get; set; }
        public List<Spare> spares = new List<Spare>();
    }

    public class Tool
    {
        public int Id { get; set; }
        public string Toolname { get; set; }
        public string Toolref { get; set; }
        public int Toolnum { get; set; }
    }

    public class ToolsList
    {
        public int Id { get; set; }
        public List<Tool> tools = new List<Tool>();
    }

    public class Requirement
    {
        public int Id { get; set; }
        public string Reqhtml { get; set; }
    }

    public class RequirementsList
    {
        public int Id { get; set; }
        public List<Requirement> requirements = new List<Requirement>();
    }

    public class Warning
    {
        public int Id { get; set; }
        public string Wtype { get; set; }
        public string Whtml { get; set; }
    }

    public class WarningsList
    {
        public int Id { get; set; }
        public List<Warning> warnings = new List<Warning>();
    }


    public class Related
    {
        public int Id { get; set; }
        public string Rtitle { get; set; }
        public string Rlink { get; set; }
    }

    public class RelatedsList
    {
        public int Id { get; set; }
        public List<Related> relateds = new List<Related>();
    }


    public class Family
    {
        public int Id { get; set; }
        public string Ftitle { get; set; }
        public string Flink { get; set; }
    }

    public class FamiliyList
    {
        public int Id { get; set; }
        public List<Family> familys = new List<Family>();
    }
}
