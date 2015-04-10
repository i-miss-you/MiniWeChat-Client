﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MiniWeChat
{
    public class ChatListPanel : BasePanel
    {
        private const int CHAT_FRAME_HEIGHT = 200;

        public VerticalLayoutGroup _chatGrid;
        public ScrollRect _scrollChatList;

        private List<ChatFrame> _chatFrameList = new List<ChatFrame>();

        public override void Show(object param = null)
        {
            base.Show(param);
            MessageDispatcher.GetInstance().RegisterMessageHandler((uint)EUIMessage.UPDATE_CHAT_LIST, OnUpdateChatList);
            RefreshChatFrames();
        }

        public override void Hide()
        {
            base.Hide();
            MessageDispatcher.GetInstance().UnRegisterMessageHandler((uint)EUIMessage.UPDATE_CHAT_LIST, OnUpdateChatList);
        }

        public void OnUpdateChatList(uint iMessageType, object kParam)
        {
            RefreshChatFrames();
        }

        public void RefreshChatFrames()
        {
            int i = 0;
            foreach (ChatLog chatLog in GlobalChat.GetInstance())
            {
                if (i >= _chatFrameList.Count)
                {
                    GameObject go = UIManager.GetInstance().AddChild(_chatGrid.gameObject, EUIType.ChatFrame);
                    _chatFrameList.Add(go.GetComponent<ChatFrame>());
                }
                _chatFrameList[i].Show(chatLog);
                i++;

                if (_chatFrameList.Count > GlobalChat.GetInstance().Count)
                {
                    for (i = GlobalChat.GetInstance().Count; i < _chatFrameList.Count; i++)
                    {
                        GameObject.Destroy(_chatFrameList[i].gameObject);
                    }
                    _chatFrameList = _chatFrameList.GetRange(0, GlobalChat.GetInstance().Count);
                }
            }

            _chatGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(GlobalVars.DEFAULT_SCREEN_WIDTH, CHAT_FRAME_HEIGHT * GlobalChat.GetInstance().Count);
            _scrollChatList.verticalNormalizedPosition = 1.0f;

            Log4U.LogDebug("ChatGrid y : ", _chatGrid.GetComponent<RectTransform>().sizeDelta.y);
            Log4U.LogDebug("GlobalChat y : ", GlobalChat.GetInstance().Count);
        }
    }
}

