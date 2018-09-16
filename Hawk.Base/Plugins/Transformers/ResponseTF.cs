﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using Hawk.Base.Crawlers;
using Hawk.Base.Utils.Plugins;
using HtmlAgilityPack;

namespace Hawk.Base.Plugins.Transformers
{
    [XFrmWork("ResponseTF", "ResponseTF_desc")]
    public class ResponseTF : TransformerBase
    {
        protected readonly BuffHelper<HtmlDocument> buffHelper = new BuffHelper<HtmlDocument>(50);
        private readonly HttpHelper helper = new HttpHelper();
        protected SmartCrawler _crawler;

        public ResponseTF()
        {
            CrawlerSelector = new TextEditSelector();
            CrawlerSelector.GetItems = this.GetAllCrawlerNames();
        }

        [LocalizedDisplayName("key_359")]
        [LocalizedDescription("key_360")]
        public TextEditSelector CrawlerSelector { get; set; }

        [LocalizedDisplayName("key_529")]
        [LocalizedDescription("key_530")]
        public virtual string HeaderFilter { get; set; }

        [Browsable(false)]
        public SmartCrawler Crawler
        {
            get { return _crawler; }
            set
            {
                if (_crawler != value)
                {
                    if (_crawler != null)
                        Crawler.PropertyChanged -= CrawlerPropertyChangedHandler;
                    value.PropertyChanged += CrawlerPropertyChangedHandler;
                    _crawler = value;
                }
            }
        }

        private void CrawlerPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            buffHelper.Clear();
        }

        public override bool Init(IEnumerable<IFreeDocument> datas)
        {
            OneOutput = false;
            Crawler = GetCrawler(CrawlerSelector.SelectItem);
            if (string.IsNullOrEmpty(CrawlerSelector.SelectItem) && Crawler != null)
                CrawlerSelector.SelectItem = Crawler.Name;
            return  base.Init(datas);
        }

        public override
            object TransformData(IFreeDocument datas)
        {
            var p = datas[Column];
            if (p == null)
                return new List<FreeDocument>();
            var url = p.ToString();
            WebHeaderCollection responseHeader;
            HttpStatusCode code;

            var content = helper.GetHtml(Crawler.Http, out responseHeader, out code, url);
            var keys = responseHeader.AllKeys;
            if (!string.IsNullOrEmpty(HeaderFilter))
            {
                keys = HeaderFilter.Split(' ');
            }
            foreach (var key in keys)
            {
                var value = responseHeader.Get(key);
                if (value != null)
                    datas.SetValue(key, value);
            }
            if (keys.Contains("Location") && datas.ContainsKey("Location") == false)
            {
                datas["Location"] = url;
            }

            return null;
        }
    }
}