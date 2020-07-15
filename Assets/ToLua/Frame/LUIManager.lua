--
LUIManager=LManagerBase:New();
LUIManager.__index=LUIManager;
function LUIManager:New(msgId)
	local self = {};
	setmetatable(self,LUIManager);
	self.msgId=msgId;
	return self;
end
function LUIManager:GetInstance()
	return this;
end
function LUIManager:SendMsg(msg)
	if msg:GetManager()==LManagerID.LUIManager then
		self:ProcessEvent(msg);
	else
		LMsgCenter.SendToMsg(msg);
	end
end