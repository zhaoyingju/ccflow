/****** 对象:  View WF_EmpWorks    脚本日期: 03/12/2011 21:42:50 ******/
/*  WF_EmpWorks  */;
CREATE VIEW  WF_EmpWorks
(
PRI,
WorkID,
IsRead,
Starter,
StarterName,
WFState, 
FK_Dept,
DeptName,
FK_Flow,
FlowName,
PWorkID,
PFlowNo,
FK_Node,
NodeName,
Title,
RDT,
ADT,
SDT,
FK_Emp,
FID,
FK_FlowSort,
SDTOfFlow,
PressTimes
)
AS
SELECT A.PRI,A.WorkID,B.IsRead, A.Starter,
A.StarterName,
A.WFState,
A.FK_Dept,A.DeptName, A.FK_Flow, A.FlowName,A.PWorkID,A.PFlowNo,
B.FK_Node, B.FK_NodeText AS NodeName, A.Title, A.RDT, B.RDT AS ADT, 
B.SDT, B.FK_Emp,B.FID ,A.FK_FlowSort,A.SDTOfFlow,B.PressTimes
FROM  WF_GenerWorkFlow A, WF_GenerWorkerList B
WHERE     (B.IsEnable = 1) AND (B.IsPass = 0)
 AND A.WorkID = B.WorkID AND A.FK_Node = B.FK_Node ;
  


/*  WF_NodeExt  */;
CREATE VIEW WF_NodeExt
(
No,
NAME
)
AS
SELECT NODEID AS NO , NAME AS NAME FROM WF_Node ;