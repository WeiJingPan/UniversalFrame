--负责各个消息模块的转发
LMsgCenter={msgId=0}
LMsgCenter.__index=LMsgCenter;
local this = 1

function LMsgCenter:New(msgId)
	local self = {};
	setmetatable(self,LMsgCenter);
	self.msgId=msgId;
	return self;
end
function LMsgCenter:GetManager()
	tmpId=math.floor(self.msgId/MsgSpan)*MsgSpan;
	return math.ceil(tmpId);
end
function LMsgCenter.GetInstance()
	return this;
end
function LMsgCenter.RecvMsg(msg)
	
end
function LMsgCenter.SendToMsg(msg)
	this.AnasysMsg(msg);
end
function LMsgCenter.AnasysMsg(msg)
	managerId=msg:GetManager();
	if managerId==LManagerID.LAssetManager then
	elseif managerId==LManagerID.LuaManager then
	elseif managerId==LManagerID.LNetManager then
	elseif managerId==LManagerID.LUIManager then
	elseif managerId==LManagerID.LNPCManager then
	elseif managerId==LManagerID.LCharactorManager then
	elseif managerId==LManagerID.LDataManager then
	elseif managerId==LManagerID.LAudioManager then
	elseif managerId==LManagerID.NetManager then
	elseif managerId==LManagerID.UIManager then
		LUIManager.GetInstance().SendMsg(msg);
	elseif managerId==LManagerID.NPCManager then
	elseif managerId==LManagerID.CharactorManager then
	elseif managerId==LManagerID.AssetManager then
	elseif managerId==LManagerID.DataManager then
	elseif managerId==LManagerID.AudioManager then
	end
end
