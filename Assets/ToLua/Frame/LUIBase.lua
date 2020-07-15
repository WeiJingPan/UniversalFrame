--
LUIBase={msgId=0}
LUIBase.__index=LUIBase;
function LUIBase:New(msgId)
	local self = {};
	setmetatable(self,LUIBase);
	self.msgId=msgId;
	return self;
end
function LUIBase:RegistSelf(script,msgs)
	LUIManager.GetInstance():RegistMsgs(script,msgs);
end
function LUIBase:UnRegistSelf(script,msgs)
	LUIManager.GetInstance():UnRegistMsgs(script,msgs);
end
function LUIBase:Destory()
	self:UnRegistSelf(self,self.msgIds);
end