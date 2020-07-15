--负责消息路由
LEventNode={msgId=0}
LEventNode.__index=LEventNode;
function LEventNode:New(msgId)
	local self = {};
	setmetatable(self,LEventNode);
	self.msgId=msgId;
	self.next=nil;
	return self;
end
function LEventNode.GetInstance()
	return this;
end
function LEventNode:RegistMsg(id,event)

end