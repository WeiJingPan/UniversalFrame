--负责消息路由
LMsgBase={msgId=0}
LMsgBase.__index=LMsgBase;
function LMsgbase:New(msgId)
	local self = {};
	setmetatable(self,LMsgBase);
	self.msgId=msgId;
	return self;
end
function LMsgbase:GetManager()
	tmpId=math.floor(self.msgId/MsgSpan)*MsgSpan;
	return math.ceil(tmpId);
end