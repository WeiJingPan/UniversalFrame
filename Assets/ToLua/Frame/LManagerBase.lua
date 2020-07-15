--负责消息存储和处理
LManagerBase={eventTree={}}
LManagerBase.__index=LManagerBase;
function LManagerBase:New(msgId)
	local self = {};
	setmetatable(self,LManagerBase);
	self.msgId=msgId;
	return self;
end

function LManagerBase:FindKey(dict,key)
	for k,v in pairs(dict) do
		if k==key then
			return true;
		end
		return false;
	end
end
function LManagerBase.GetInstance()
	return this;
end
--注册单个消息
function LManagerBase:RegistMsg(id,eventNode)
	if this:FindKey(self.eventTree,id) then
		self.eventTree[id]=eventNode;
	else
		tmpNode=self.eventTree[id];
		while tmpNode.next~=nil do
			tmpNode=tmpNode.next;
		end
		tmpNode.next=eventNode;
	end
end
--注册一个脚本的若干条消息
function LManagerBase:RegistMsgs(script,msg)
	for k,v in pairs(table_name) do
		eventNode=LEventNode:New(script);
		self:RegistMsg(v,eventNode);
	end
end
function LManagerBase:UnRegistMsg(script,id)
	if this:FindKey(self.eventTree.id) then
		tmpNode=self.eventTree[id];
		if tmpNode.value==script then
			if tmpNode.next~=nil then
				self.eventTree[id]=tmpNode.next;
				tmpNode.next=nil;
				else
					while tmpNode.next~=nil and tmpNode.next.value ~=script do
						tmpNode=tmpNode.next;
					end
					--如果后面还有
					if tmpNode.next.next~=nil then
						curNode=tmpNode.next;
						--指向的删除
						tmpNode.next=curNode.next;
						--下个删除
						curNode.next=nil;
						else
							tmpNode.next=nil;
						end
				end
			end
		end
	end
end
function LManagerBase:UnRegistMsgs(script,...)
	if args==nil then
	return;
	end
	for i in args do
		self.UnRegistMsg(script,k);
	end
end
function LManagerBase:Destory()
	keys={}
	keyCount=0;
	for k,v in pairs(self.eventTree) do
		keys[keyCount]=k;
		keyCount=keyCount+1;
	end
	for i=1,keyCount do
		self.eventTree[keys[i]]=nil;
	end
end
function LManagerBase:ProcessEvent(msg)
	if this.FindKey(self.eventTree,msg.msgId) then
		local tmpNode = self.eventTree[msg.msgId];
		while tmpNode~=nil do
			tmpNode.value:ProcessEvent(msg);
			if tmpNode.next~=nil then
				tmpNode=tmpNode.next;
			end
		end
	else
		print("Msg not contain msg =="..msg.msgId);
	end
end