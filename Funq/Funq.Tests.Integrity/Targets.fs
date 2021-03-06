﻿module Funq.Tests.Integrity.Target
let lst n = [0..n]
module Funq = 
    let List n = FunqListWrapper<_>.FromSeq (lst n)
    let Vector n= FunqVectorWrapper<_>.FromSeq (lst n)
    let Set = FunqSetWrapper<_>.FromSeq 
    let OrderedSet = FunqOrderedSetWrapper<_>.FromSeq 
    let Map s = FunqMapWrapper<_>.FromSeq s
    let OrderedMap s = FunqOrderedMapWrapper<_>.FromSeq s

module Sys = 
    let List n= SeqReferenceWrapper<_>.FromSeq (lst n)
    let OrderedSet = ReferenceSetWrapper<_>.FromSeq
    let OrderedMap = MapReferenceWrapper<_>.FromSeq

