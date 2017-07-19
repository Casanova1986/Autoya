using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace AutoyaFramework.Information {
    
    /**
        parsed tree structure.
     */
    public class ParsedTree {
        // tree params.
        private List<ParsedTree> _childlen = new List<ParsedTree>();
        
        // tag params.
        public readonly int parsedTag;
        public readonly string[] depth;
        public readonly string rawTagName;
        public readonly string prefabName;
        public readonly AttributeKVs keyValueStore;
        public readonly bool isContainer;

        // layout things.
        public Vector2 anchoredPosition = Vector2.zero;        
        public Vector2 sizeDelta = Vector2.zero;
        public Vector2 offsetMin = Vector2.zero;
        public Vector2 offsetMax = Vector2.zero;
        public Padding padding = new Padding();

        public ParsedTree () {
            this.parsedTag = (int)HtmlTag._ROOT;
            this.depth = new string[0];
            this.keyValueStore = new AttributeKVs();
            this.rawTagName = HtmlTag._ROOT.ToString();

            this.prefabName = HtmlTag._ROOT.ToString();
        }

        public ParsedTree (int parsedTag, ParsedTree parent, AttributeKVs kv, string rawTagName) {
            this.parsedTag = parsedTag;
            this.depth = parent.depth.Concat(new string[]{rawTagName}).ToArray();
            this.keyValueStore = kv;
            this.rawTagName = rawTagName;

            /*
                determine which tag should be load.
             */
            switch (parsedTag) {
                // this is content of container = parent tag.
                case (int)HtmlTag._TEXT_CONTENT: {
                    this.prefabName = parent.rawTagName;
                    this.isContainer = false;
                    break;
                }

                // value tags,
                case (int)HtmlTag.BODY:
                case (int)HtmlTag.IMG:
                case (int)HtmlTag.BR:
                case (int)HtmlTag.HR:

                // and system tags are only prefab.
                case (int)HtmlTag._ROOT:
                case (int)HtmlTag._DEPTH_ASSET_LIST_INFO: {
                    this.prefabName =  rawTagName.ToUpper();
                    this.isContainer = false;
                    break;
                }

                // other tags are container.
                default: {
                    this.prefabName = rawTagName.ToUpper() + InformationConstSettings.NAME_PREFAB_CONTAINER;
                    this.isContainer = true;
                    break;
                }
            }
        }

        public ParsedTree (ParsedTree source, AttributeKVs kv) {
            this.parsedTag = source.parsedTag;
            this.depth = source.depth;
            this.keyValueStore = kv;
            this.rawTagName = source.rawTagName;

            this.prefabName = source.prefabName;
        }
        
        public void SetParent (ParsedTree t) {
            if (
                t.parsedTag == (int)HtmlTag._ROOT && 
                this.parsedTag == (int)HtmlTag._TEXT_CONTENT
            ) {
                var val = this.keyValueStore[Attribute._CONTENT];
                throw new Exception("invalid text contains outside of tag. val:" + val);
            }
            
			t._childlen.Add(this);
		}

        public List<ParsedTree> GetChildlen () {
			return _childlen;
		}

        public void SetChildlen (List<ParsedTree> childlen) {
            this._childlen = childlen;
        }
    }
}